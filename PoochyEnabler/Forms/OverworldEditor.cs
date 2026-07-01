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

        private static class StateKeys
        {
            public static readonly string d1 = nameof(d1);
            public static readonly string d2 = nameof(d2);
        }

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
            // InitializeControls();
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
                        owDataOffset != Constants.InvalidOffset)
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

        private void LoadEntryToUI(int idx)
        {
            _isUpdatingUI = true;
            _reservationManager.ClearAllReservations();

            _isUpdatingUI = false;
            _stateManager.SetInitialValues();
        }
    }
}
