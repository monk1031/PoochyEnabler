using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using PoochyEnabler.FileReaders;
using PoochyEnabler.Helpers;
using PoochyEnabler.Managers;

namespace PoochyEnabler.Forms
{
    public partial class OverworldEditor : Form
    {
        private readonly byte[] _romData = null;
        private readonly IniFileReader _config = null;
        private readonly TblFileReader _charmap = null;
        private readonly ReservationManager _reservationManager = null;
        private readonly StateManager _stateManager = null;
        private readonly Action _saveAction = null;

        private EntryManager<OverworldDataEntry> _owManager = null;
        private EntryManager<OverworldPaletteEntry> _palManager = null;
        private EntryManager<OverworldFrameEntry> _FrameManager = null;

        private bool _isUpdatingUI = false;
        private int _currentTableIdx = 0;
        private int _currentOwIdx = 0;
        private bool _isMultipleTable = false;
        private Dictionary<int, int> _groupPointers = null;
        private Dictionary<int, int> _dataPointers = null;
        private BindingList<PaletteComboItem> _paletteComboSource;

        private static class StateKeys
        {
            public static readonly string d1 = nameof(d1);
            public static readonly string d2 = nameof(d2);
        }

        private class PaletteComboItem
        {
            public ushort PaletteIdx { get; set; }
            public int? TableIdx { get; set; } // dummy = null, temporary = -1
            public int PaletteOffset { get; set; }
            public string DisplayText => 
                $"{PaletteIdx:X4}" + (TableIdx == -1 ? "*" : ""); // temporary -> *
        }

        private class FrameSizeComboItem
        {
            public string Key { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public int VramSize { get; set; }
            public string DisplayText => $"{Key} (0x{VramSize:X})";
        }

        private List<FrameSizeComboItem> _dataSizePresets = new List<FrameSizeComboItem>
        {
            new FrameSizeComboItem { Key = "16x32",  Width = 0x10, Height = 0x20, VramSize = 0x100 },
            new FrameSizeComboItem { Key = "32x32",  Width = 0x20, Height = 0x20, VramSize = 0x200 },
            new FrameSizeComboItem { Key = "16x16",  Width = 0x10, Height = 0x10, VramSize = 0x80 },
            new FrameSizeComboItem { Key = "64x64",  Width = 0x40, Height = 0x40, VramSize = 0x800 },
            new FrameSizeComboItem { Key = "128x64", Width = 0x80, Height = 0x40, VramSize = 0x1000 },
            new FrameSizeComboItem { Key = "32x16",  Width = 0x20, Height = 0x10, VramSize = 0x100 }
        };

        public OverworldEditor(
            byte[] romData,
            IniFileReader config,
            TblFileReader charmap,
            ReservationManager reservationManager,
            StateManager stateManager,
            Action saveAction)
        {
            InitializeComponent();
            _romData = romData;
            _config = config;
            _charmap = charmap;
            _reservationManager = reservationManager;
            _stateManager = stateManager;
            _saveAction = saveAction;

            InitializeManagers();
            InitializeControls();
            // InitializeEventHandlers();

            LoadEntryToUI(_currentTableIdx);
        }

        private void InitializeManagers()
        {
            _isMultipleTable = _config.TryReadValue("EnableMultipleOverworldSpriteTable", out bool enabled) && enabled;
            int maxEntries = (int)byte.MaxValue;
            string patternPtr = "ptr";
            _groupPointers = new Dictionary<int, int>();

            if (_isMultipleTable && _config.TryReadValue("MultipleOverworldSpriteTableOffset", out int groupPtr))
            {
                // count ow table group
                int groupCount = EntryCountHelper.Count(_romData, groupPtr, patternPtr, maxEntries, true);
                for (int i = 0; i < groupCount; i++)
                {
                    int currentGroupPtr = groupPtr + (i * Constants.UIntSize);
                    if (IOHelper.TryReadPtr(currentGroupPtr, _romData, out int owTableOffset))
                    {
                        _groupPointers.Add(i, owTableOffset);
                    }
                }
            }
            else // single group
            {
                if (_config.TryReadValue("OverworldSpriteTableOffset", out int owTableOffset))
                {
                    _groupPointers.Add(0, owTableOffset); // index 0
                }
            }

            // count entry
            _dataPointers = new Dictionary<int, int>();
            foreach (var owTableDict in _groupPointers)
            {
                int owTableOffset = owTableDict.Value;
                int entryCount = EntryCountHelper.Count(_romData, owTableOffset, patternPtr, maxEntries, true);
                for (int i = 0; i < entryCount; i++)
                {
                    int currentEntryPtr = owTableOffset + (i * Constants.UIntSize);
                    if (IOHelper.TryReadPtr(currentEntryPtr, _romData, out int owDataOffset) && 
                        owDataOffset != Constants.InvalidOffset) // null pointer
                    {
                        _dataPointers.Add(i, owDataOffset);
                    }
                }
            }

            // valid entry?
            var keysToRemove = new List<int>();
            string patternEntry = "FF FF ?? 11 ?? 11 ?? ?? ?? 00 ?? 00 ?? ?? ?? 00 ptr ptr ptr ptr ptr";
            foreach (var owDataDict in _dataPointers)
            {
                if (!EntryCountHelper.Validate(_romData, owDataDict.Value, patternEntry, true))
                {
                    keysToRemove.Add(owDataDict.Key);
                }
            }
            foreach (var key in keysToRemove)
            {
                _dataPointers.Remove(key);
            }

            // palette
            patternEntry = "ptr ?? 11 00 00";
            if (_config.TryReadValue("OverworldSpritePaletteTableOffset", out int palTableOffset))
            {
                int palCount = EntryCountHelper.Count(_romData, palTableOffset, patternEntry, maxEntries, true);
                _palManager = new EntryManager<OverworldPaletteEntry>(_romData, _config, _charmap);
                _palManager.Load(palTableOffset, palCount);
            }

            _stateManager.StateChanged += hasChanges => btnSave.Enabled = hasChanges;
            _stateManager.AddControls(
                txtEntryOffset);
            _stateManager.AddControlsRecursive(
                grpEntryData);
            _stateManager.AddBinaries(
                (StateKeys.d1, null),
                (StateKeys.d2, null));


            // _owManager = new EntryManager<OverworldEntry>(_romData, _config, _charmap);
            // _owManager.Load("TrainerImageTableOffset", "TrainerSpriteCount");

            // _FrameManager = new EntryManager<OverworldFrameEntry>(_romData, _config, _charmap);
            // _FrameManager.Load("TrainerYPositionTableOffset", "TrainerSpriteCount");
        }

        private void InitializeControls()
        {
            // nudTableIdx
            nudTableIdx.Maximum = _dataPointers.Count - 1;

            // frame size cmb
            cmbFrameSize.DataSource = new List<FrameSizeComboItem>(_dataSizePresets);
            cmbFrameSize.DisplayMember = nameof(FrameSizeComboItem.DisplayText);

            // picSpritePreviewFrame
            picPreview.SizeMode = PictureBoxSizeMode.CenterImage;
            picPreview.BackColor = Color.White;

            ControlHelper.AttachOffsetAutoFormat(
                (txtUnkPtr1, false), 
                (txtSizeDrawPtr, false), 
                (txtShiftRedrawPtr, false), 
                (txtUnkPtr2, false));
            ControlHelper.AttachExternalBorder(
                picPreview,
                picPalettePreview);
            ControlHelper.AttachNumericUpDownNavigators(
                nudPreviewIdx, btnPreviewPrev, btnPreviewNext);
            ControlHelper.LoadComboBoxFromTextFile(cmbFootPrint, "txt/OverworldSpriteFootprint.txt");
            ControlHelper.LoadComboBoxFromTextFile(cmbTextColor, "txt/OverworldSpriteTextColor.txt");

            UpdatePaletteComboBox();
        }

        private void UpdatePaletteComboBox()
        {
            var items = new List<PaletteComboItem>();

            // exist
            for (int i = 0; i < _palManager.Entries.Count; i++)
            {
                var palEntry = _palManager.Entries[i];

                // register
                items.Add(new PaletteComboItem
                {
                    PaletteIdx = _palManager.Entries[i]._PaletteIdx,
                    TableIdx = i,
                    PaletteOffset =
                        palEntry.pPaletteOffset == 0
                        ? Constants.InvalidOffset
                        : (int)(palEntry.pPaletteOffset - Constants.BaseAddr)
                });
            }

            // 11FF (dummy)
            if (!items.Any(x => x.PaletteIdx == 0x11FF))
            {
                items.Add(new PaletteComboItem
                {
                    PaletteIdx = 0x11FF,
                    TableIdx = null,
                    PaletteOffset = Constants.InvalidOffset
                });
            }

            // sort
            var sortedItems = items.OrderBy(x => x.PaletteIdx).ToList();
            _paletteComboSource = new BindingList<PaletteComboItem>(sortedItems);

            foreach (var cmb in new[] {
                cmbPaletteIdx1,
                cmbPaletteIdx2,
                cmbPaletteIdx3 })
            {
                var bindingSource = new BindingSource();
                bindingSource.DataSource = _paletteComboSource;

                cmb.DisplayMember = nameof(PaletteComboItem.DisplayText);
                cmb.ValueMember = nameof(PaletteComboItem.TableIdx);
                cmb.DataSource = bindingSource;
            }
        }

        private void LoadEntryToUI(int idx)
        {
            _isUpdatingUI = true;
            _reservationManager.ClearAllReservations();

            _isUpdatingUI = false;
            _stateManager.SetInitialValues();
        }
    }
}
