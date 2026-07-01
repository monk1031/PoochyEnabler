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
            InitializeControls();
            InitializeEventHandlers();

            LoadEntryToUI(_currentOwIdx);
        }

        private void InitializeManagers()
        {
            _isMultipleTable = _config.TryReadValue("EnableMultipleOverworldSpriteTable", out bool enabled) && enabled;
            if (_isMultipleTable && _config.TryReadValue("MultipleOverworldSpriteTableOffset", out int groupOffset))
            {
                int maxEntries = (int)byte.MaxValue;

                // count ow table group
                string pattern = "ptr";
                int groupCount = EntryCountHelper.Count(_romData, groupOffset, pattern, maxEntries, true);
                for (int i = 0; i < groupCount; i++)
                {
                    groupOffset += i * Constants.UIntSize;
                    if (IOHelper.TryReadPtr(groupOffset, _romData, out int owTableOffset))
                    {
                        _groupPointers.Add(i, owTableOffset);
                    }
                }

                // count entry
                foreach (var groupPtr in _groupPointers)
                {
                    int owTableOffset = groupPtr.Value;
                    int entryCount = EntryCountHelper.Count(_romData, owTableOffset, pattern, maxEntries, true);
                    
                    for (int i = 0; i < entryCount; i++)
                    {
                        owTableOffset += i * Constants.UIntSize;
                        if (IOHelper.TryReadPtr(groupOffset, _romData, out int owDataOffset))
                        {
                            _dataPointers.Add(i, owDataOffset);
                        }
                    }
                }

                // valid entry?
                pattern = "FF FF ?? 11 ?? 11 ?? ?? ?? 00 ?? 00 ?? ?? ?? 00 ptr ptr ptr ptr ptr";






            }




            _owManager = new EntryManager<OverworldEntry>(_romData, _config, _charmap);
            _owManager.Load("TrainerImageTableOffset", "TrainerSpriteCount");
            _palManager = new EntryManager<OverworldPaletteEntry>(_romData, _config, _charmap);
            _palManager.Load("TrainerPaletteTableOffset", "TrainerSpriteCount");




            _FrameManager = new EntryManager<OverworldFrameEntry>(_romData, _config, _charmap);
            _FrameManager.Load("TrainerYPositionTableOffset", "TrainerSpriteCount");


            _stateManager.StateChanged += hasChanges => btnSave.Enabled = hasChanges;
            _stateManager.AddControls(
                txtImageOffset,
                txtPaletteOffset,
                nudYPosition);
            _stateManager.AddBinaries(
                (StateKeys.ImageData, null),
                (StateKeys.PaletteData, null));
        }
    }
}
