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
        private int _currentEntryIdx = 0;
        private bool _isMultipleTable = false;
        private List<Dictionary<int, int>> _dataPointers = null;
        private BindingList<PaletteComboItem> _paletteComboSource = null;

        private const byte OverworldSpriteUnkFlag1Mask = 0x10;
        private const byte OverworldSpriteUnkFlag2Mask = 0x40;
        private const byte OverworldSpriteUnkFlag3Mask = 0x80;

        private static class StateKeys
        {
            public static readonly string SpriteFrames = nameof(SpriteFrames);
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
            InitializeEventHandlers();

            LoadDataEntryListBox(_currentTableIdx);
            LoadEntryToUI(_currentEntryIdx);
        }

        private void InitializeManagers()
        {
            _isMultipleTable = _config.TryReadValue("EnableMultipleOverworldSpriteTable", out bool enabled) && enabled;
            int maxEntries = (int)byte.MaxValue;
            string patternStr = "ptr";
            _dataPointers = new List<Dictionary<int, int>>();

            var owTableOffsets = new List<int>(); // temporary
            if (_isMultipleTable && _config.TryReadValue("MultipleOverworldSpriteTableOffset", out int baseGroupOffset))
            {
                // count ow table group
                int groupCount = EntryCountHelper.Count(_romData, baseGroupOffset, patternStr, maxEntries, true);
                for (int i = 0; i < groupCount; i++)
                {
                    int currentGroupOffset = baseGroupOffset + (i * Constants.UIntSize);
                    if (IOHelper.TryReadPtr(currentGroupOffset, _romData, out int owTableOffset))
                    {
                        owTableOffsets.Add(owTableOffset); // include null pointer
                    }
                }
            }
            else // single group
            {
                if (_config.TryReadValue("OverworldSpriteTableOffset", out int owTableOffset))
                {
                    owTableOffsets.Add(owTableOffset); // index 0
                }
            }

            // add entry
            foreach (int owTableOffset in owTableOffsets)
            {
                Dictionary<int, int> owEntryOffsets = null;

                // null pointer?
                if (owTableOffset == Constants.InvalidOffset)
                {
                    _dataPointers.Add(owEntryOffsets);
                    continue;
                }

                owEntryOffsets = new Dictionary<int, int>();
                int entryCount = EntryCountHelper.Count(_romData, owTableOffset, patternStr, maxEntries, true);
                for (int i = 0; i < entryCount; i++)
                {
                    int currentEntryOffset = owTableOffset + (i * Constants.UIntSize);
                    if (IOHelper.TryReadPtr(currentEntryOffset, _romData, out int owDataOffset))
                    {
                        owEntryOffsets.Add(i, owDataOffset);
                    }
                }
                _dataPointers.Add(owEntryOffsets);
            }

            // valid entry?
            patternStr = "FF FF ?? 11 ?? 11 ?? ?? ?? 00 ?? 00 ?? ?? ?? 00 ptr ptr ptr ptr ptr";
            foreach (var owTableIdx in _dataPointers)
            {
                var keysToRemove = new List<int>();
                foreach (var kvp in owTableIdx)
                {
                    int targetOffset = kvp.Value;
                    if (targetOffset == Constants.InvalidOffset || // remove null entry
                        !EntryCountHelper.Validate(_romData, targetOffset, patternStr, true))
                    {
                        keysToRemove.Add(kvp.Key);
                    }
                }

                foreach (var key in keysToRemove)
                {
                    owTableIdx.Remove(key);
                }
            }

            // palette
            patternStr = "ptr ?? 11 00 00";
            if (_config.TryReadValue("OverworldSpritePaletteTableOffset", out int palTableOffset))
            {
                int palCount = EntryCountHelper.Count(_romData, palTableOffset, patternStr, maxEntries, true);
                _palManager = new EntryManager<OverworldPaletteEntry>(_romData, _config, _charmap);
                _palManager.Load(palTableOffset, palCount);
            }

            _stateManager.StateChanged += hasChanges => btnSave.Enabled = hasChanges;
            _stateManager.AddControls(
                txtEntryOffset);
            _stateManager.AddControlsRecursive(
                grpEntryData);
            _stateManager.AddBinaries(
                (StateKeys.SpriteFrames, null),
                (StateKeys.d2, null));

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
            ControlHelper.LoadComboBoxFromTextFile(cmbFootprint, "txt/OverworldSpriteFootprint.txt");
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

        private void InitializeEventHandlers()
        {
            // btnSave.Click += btnSave_Click;
            // this.FormClosing += OverworldSpriteEditor_FormClosing;

            // nudDataTableIdx.ValueChanged += nudDataTableIdx_ValueChanged;
            lstEntry.SelectedIndexChanged += lstEntry_SelectedIndexChanged;

            // cmbDataPalIdx1.SelectedIndexChanged += cmbDataPalIdx1_SelectedIndexChanged;
            // nudSpriteFrameCount.ValueChanged += nudSpriteFrameCount_ValueChanged;

            // cmbPalIdx.SelectedIndexChanged += cmbPalIdx_SelectedIndexChanged;
            // btnCreateNewPalIdx.Click += btnCreateNewPalIdx_Click;

            // btnImportDataEntry.Click += btnImportDataEntry_Click;
            // btnExportDataEntry.Click += btnExportDataEntry_Click;
            // btnCreateNewDataEntry.Click += btnCreateNewDataEntry_Click;

            // btnCreateNewImgTable.Click += btnCreateNewImgTable_Click;

            // btnImportSpriteFrames.Click += btnImportSpriteFrames_Click;
            // btnExportSpriteFrames.Click += btnExportSpriteFrames_Click;
        }

        private void LoadDataEntryListBox(int idx)
        {
            lstEntry.BeginUpdate();
            lstEntry.Items.Clear();
            int entryCount = _dataPointers[idx].Keys.Max() + 1;
            for (int i = 0; i < entryCount; i++)
            {
                lstEntry.Items.Add($"No. {i:D4}");
            }
            lstEntry.EndUpdate();

            _isUpdatingUI = true;
            if (lstEntry.Items.Count > 0)
            {
                lstEntry.SelectedIndex = 0;
            }
            _isUpdatingUI = false;
        }

        private void LoadEntryToUI(int idx)
        {
            _isUpdatingUI = true;
            _reservationManager.ClearAllReservations();

            _currentEntryIdx = idx;

            _owManager = new EntryManager<OverworldDataEntry>(_romData, _config, _charmap);
            _owManager.Load(_dataPointers[_currentTableIdx][idx], 1); // this entry
            var currentEntry = _owManager.Entries[0];

            // txtEntryOffset
            txtEntryOffset.Text = _owManager.Offset.ToString("X8");

            // grpEntryData
            BindingHelper.BindObjectToControls(grpEntryData, currentEntry);

            // palette cmb
            LoadToPaletteComboBox(cmbPaletteIdx1, currentEntry._PaletteIdx1);
            LoadToPaletteComboBox(cmbPaletteIdx2, currentEntry._PaletteIdx2);

            // frame size cmb
            ushort width = currentEntry._FrameSizeWidth;
            ushort height = currentEntry._FrameSizeHeight;
            LoadToFrameSizeComboBox(width, height);

            // palette slot and unknown flags
            byte paletteSlotAndUnknown = currentEntry._PaletteSlotAndUnkFlags;
            nudPaletteSlot.Value = paletteSlotAndUnknown & Constants.NibbleMask;
            chkUnkFlag1.Checked = (paletteSlotAndUnknown & OverworldSpriteUnkFlag1Mask) != 0;
            chkUnkFlag2.Checked = (paletteSlotAndUnknown & OverworldSpriteUnkFlag2Mask) != 0;
            chkUnkFlag3.Checked = (paletteSlotAndUnknown & OverworldSpriteUnkFlag3Mask) != 0;

            // grpPreview
            LoadSpriteFrames();


            _isUpdatingUI = false;
            _stateManager.SetInitialValues();
        }

        private void LoadToPaletteComboBox(ComboBox cmb, ushort palIdx)
        {
            bool isFound = false;

            for (int i = 0; i < cmb.Items.Count; i++)
            {
                var item = (PaletteComboItem)cmb.Items[i];

                if (item.PaletteIdx == palIdx)
                {
                    cmb.SelectedIndex = i;
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
            {
                cmb.SelectedIndex = -1;
            }
        }

        private void LoadToFrameSizeComboBox(ushort width, ushort height)
        {
            bool isFound = false;

            for (int i = 0; i < cmbFrameSize.Items.Count; i++)
            {
                var item = (FrameSizeComboItem)cmbFrameSize.Items[i];

                if (item.Width == width && item.Height == height)
                {
                    cmbFrameSize.SelectedIndex = i;
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
            {
                cmbFrameSize.SelectedIndex = -1;
            }
        }

        private void LoadSpriteFrames()
        {
            // clear
            picPreview?.Dispose();
            picPreview.Image = null;

            // check offset
            bool isImgTableValid = 
                !string.IsNullOrWhiteSpace(txtSpriteTablePtr.Text) &&
                !txtSpriteTablePtr.Text.Equals("null");
            ControlHelper.SetControlsEnabled(picPreview, isImgTableValid);
            if(!isImgTableValid) // addtional reset
            {
                _isUpdatingUI = true;
                ControlHelper.ResetControls(picPreview);
                nudPreviewIdx.Maximum = 0;
                _isUpdatingUI = false;
                return;
            }
            if (!ControlHelper.TryParseOffset(txtSpriteTablePtr.Text, out int frameTableOffset)) return;

            // frame size
            var frameSize = cmbFrameSize.SelectedItem as FrameSizeComboItem;
            if (frameSize == null) return;
            int expectedVramSize = frameSize.VramSize;
            int expectedWidth = frameSize.Width;
            int expectedHeight = frameSize.Height;
        }

        private void lstEntry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUI) return;

            int newentryIndex = lstEntry.SelectedIndex;
            if (newentryIndex == _currentEntryIdx) return;

            if (btnSave.Enabled)
            {
                ControlHelper.HandleUnsavedChanges(
                    saveAction: () =>
                    {
                        // SaveCurrentData(_currentEntryIdx);
                        LoadEntryToUI(newentryIndex);
                    },
                    discardAction: () =>
                    {
                        // DiscardTemporaryPalette();
                        LoadEntryToUI(newentryIndex);
                    },
                    cancelAction: () =>
                    {
                        _isUpdatingUI = true;
                        lstEntry.SelectedIndex = _currentEntryIdx;
                        _isUpdatingUI = false;
                    }
                );
            }
            else
            {
                LoadEntryToUI(newentryIndex);
            }
        }
    }
}



