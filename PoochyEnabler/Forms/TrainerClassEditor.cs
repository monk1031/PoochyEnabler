using System;
using System.Linq;
using System.Windows.Forms;

using PoochyEnabler.FileReaders;
using PoochyEnabler.Managers;

namespace PoochyEnabler.Forms
{
    public partial class TrainerClassEditor : Form
    {
        private readonly byte[] _romData;
        private readonly IniFileReader _config;
        private readonly TblFileReader _charmap;
        private readonly ReservationManager _reservationManager;
        private readonly Action _saveAction;

        private EntryManager<ClassNameEntry> _nameManager;
        private EntryManager<ClassPrizeMultiplierEntry> _prizeMultiManager;
        private EntryManager<ClassEncounterMusicEntry> _encounterMusicManager;
        private EntryManager<ClassBattleMusicEntry> _battleMusicManager;
        private EntryManager<ClassPokeBallEntry> _pokeBallManager;
        private EntryManager<ClassBaseIVEntry> _baseIvManager;

        public TrainerClassEditor(
            byte[] romData,
            IniFileReader config,
            TblFileReader charmap,
            ReservationManager reservationManager,
            Action saveAction)
        {
            InitializeComponent();
            _romData = romData;
            _config = config;
            _charmap = charmap;
            _reservationManager = reservationManager;
            _saveAction = saveAction;

            InitializeManagers();
            InitializeControls();
            InitializeUIStates();
            InitializeEventHandlers();

        }

        private void InitializeManagers()
        {
            _nameManager = new EntryManager<ClassNameEntry>(_romData, _config, _charmap);
            _nameManager.Load("ClassNameTableOffset", "ClassNameCount", true);
            _prizeMultiManager = new EntryManager<ClassPrizeMultiplierEntry>(_romData, _config, _charmap);
            _prizeMultiManager.Load("ClassPrizeMultiTableOffset", "ClassPrizeMultiCount");

            if (_config.TryReadValue("EnableClassEncounterMusic", out bool enabled) && enabled)
            {
                _encounterMusicManager = new EntryManager<ClassEncounterMusicEntry>(_romData, _config, _charmap);
                _encounterMusicManager.Load("ClassEncounterMusicTableAddress", "ClassNameCount");
            }

            if (_config.TryReadValue("EnableClassBattleMusic", out enabled) && enabled)
            {
                _battleMusicManager = new EntryManager<ClassBattleMusicEntry>(_romData, _config, _charmap);
                _battleMusicManager.Load("ClassBattleMusicTableAddress", "ClassNameCount");
            }

            if (_config.TryReadValue("EnableClassPokeBall", out enabled) && enabled)
            {
                _pokeBallManager = new EntryManager<ClassPokeBallEntry>(_romData, _config, _charmap);
                _pokeBallManager.Load("ClassPokeBallTableAddress", "ClassNameCount");
            }

            if (_config.TryReadValue("EnablesClassBaseIV", out enabled) && enabled)
            {
                _baseIvManager = new EntryManager<ClassBaseIVEntry>(_romData, _config, _charmap);
                _baseIvManager.Load("ClassBaseIVTableAddress", "ClassNameCount");
            }
        }

        private void InitializeControls()
        {
            // cmbClassIdx
            var classNames = _nameManager.Entries
                             .Select(entry => entry._ClassName)
                             .ToArray();
            cmbClassIdx.Items.AddRange(classNames);
        }

        private void InitializeUIStates()
        {

        }

        private void InitializeEventHandlers()
        {

        }
    }
}
