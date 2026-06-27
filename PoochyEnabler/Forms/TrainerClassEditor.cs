using System;
using System.Linq;
using System.Windows.Forms;

using PoochyEnabler.FileReaders;
using PoochyEnabler.Helpers;
using PoochyEnabler.Managers;

namespace PoochyEnabler.Forms
{
    public partial class TrainerClassEditor : Form
    {
        private readonly byte[] _romData = null;
        private readonly IniFileReader _config = null;
        private readonly TblFileReader _charmap = null;
        private readonly ReservationManager _reservationManager = null;
        private readonly StateManager _stateManager = null;
        private readonly Action _saveAction = null;

        private EntryManager<ClassNameEntry> _nameManager = null;
        private EntryManager<ClassPrizeMultiplierEntry> _prizeMultiManager = null;
        private EntryManager<ClassEncounterMusicEntry> _encounterMusicManager = null;
        private EntryManager<ClassBattleMusicEntry> _battleMusicManager = null;
        private EntryManager<ClassPokeBallEntry> _pokeBallManager = null;
        private EntryManager<ClassBaseIVEntry> _baseIvManager = null;

        private bool _isUpdatingUI = false;
        private int _currentClassIdx = 0;

        private bool isEncounterMusicEnabled = false;
        private bool isBattleMusicEnabled = false;
        private bool isPokeBallEnabled = false;
        private bool isBaseIvEnabled = false;

        public TrainerClassEditor(
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

            LoadDataToUI(_currentClassIdx);
        }

        private void InitializeManagers()
        {
            _nameManager = new EntryManager<ClassNameEntry>(_romData, _config, _charmap);
            _nameManager.Load("ClassNameTableOffset", "ClassNameCount", true);
            _prizeMultiManager = new EntryManager<ClassPrizeMultiplierEntry>(_romData, _config, _charmap);
            _prizeMultiManager.Load("ClassPrizeMultiTableOffset", "ClassPrizeMultiCount");

            // set flags
            isEncounterMusicEnabled = _config.TryReadValue("EnableClassEncounterMusic", out bool enabled) && enabled;
            isBattleMusicEnabled = _config.TryReadValue("EnableClassBattleMusic", out enabled) && enabled;
            isPokeBallEnabled = _config.TryReadValue("EnableClassPokeBall", out enabled) && enabled;
            isBaseIvEnabled = _config.TryReadValue("EnableClassBaseIV", out enabled) && enabled;

            if (isEncounterMusicEnabled)
            {
                _encounterMusicManager = new EntryManager<ClassEncounterMusicEntry>(_romData, _config, _charmap);
                _encounterMusicManager.Load("ClassEncounterMusicTableOffset", "ClassNameCount");
            }

            if (isBattleMusicEnabled)
            {
                _battleMusicManager = new EntryManager<ClassBattleMusicEntry>(_romData, _config, _charmap);
                _battleMusicManager.Load("ClassBattleMusicTableOffset", "ClassNameCount");
            }

            if (isPokeBallEnabled)
            {
                _pokeBallManager = new EntryManager<ClassPokeBallEntry>(_romData, _config, _charmap);
                _pokeBallManager.Load("ClassPokeBallTableOffset", "ClassNameCount");
            }

            if (isBaseIvEnabled)
            {
                _baseIvManager = new EntryManager<ClassBaseIVEntry>(_romData, _config, _charmap);
                _baseIvManager.Load("ClassBaseIVTableOffset", "ClassNameCount");
            }

            _stateManager.StateChanged += hasChanges => btnSave.Enabled = hasChanges;
            _stateManager.AddControlsRecursive(
                grpClassData,
                grpExtraData);
        }

        private void InitializeControls()
        {
            // cmbClassIdx
            var classNames = _nameManager.Entries
                             .Select(entry => entry._ClassName)
                             .ToArray();
            cmbClassIdx.Items.AddRange(classNames);

            // grpExtraData
            if (!isEncounterMusicEnabled)
            {
                lblClassEncounterMusic.Enabled = false;
                nudClassEncounterMusic.Enabled = false;
            }

            if (!isBattleMusicEnabled)
            {
                lblClassBattleMusic.Enabled = false;
                nudClassBattleMusic.Enabled = false;
            }

            if (!isPokeBallEnabled)
            {
                lblClassPokeBall.Enabled = false;
                nudClassPokeBall.Enabled = false;
            }

            if (!isBaseIvEnabled)
            {
                lblClassBaseIV.Enabled = false;
                nudClassBaseIV.Enabled = false;
            }
        }

        private void InitializeEventHandlers()
        {
            btnSave.Click += btnSave_Click;
            this.FormClosing += TrainerClassEditor_FormClosing;

            cmbClassIdx.SelectedIndexChanged += cmbClassIdx_SelectedIndexChanged;
            txtClassName.TextChanged += txtClassName_TextChanged;
        }

        private void LoadDataToUI(int idx)
        {
            _isUpdatingUI = true;

            _currentClassIdx = idx;
            cmbClassIdx.SelectedIndex = idx;
            nudClassIdx.Value = (decimal)idx;

            // class name
            txtClassName.Text = _nameManager.Entries[idx]._ClassName;

            // prize multi
            var prizeEntry = _prizeMultiManager.Entries.FirstOrDefault(e => e._ClassIdx == idx);
            if (prizeEntry == null) // -> 0xFF
            {
                prizeEntry = _prizeMultiManager.Entries.FirstOrDefault(e => e._ClassIdx == 0xFF);
            }
            nudClassPrizeMulti.Value = prizeEntry?._ClassPrizeMulti ?? nudClassPrizeMulti.Minimum;

            // extra data
            if (isEncounterMusicEnabled)
            {
                BindingHelper.BindObjectToControls(grpExtraData, _encounterMusicManager.Entries[idx]);
            }

            if (isBattleMusicEnabled)
            {
                BindingHelper.BindObjectToControls(grpExtraData, _battleMusicManager.Entries[idx]);
            }

            if (isPokeBallEnabled)
            {
                BindingHelper.BindObjectToControls(grpExtraData, _pokeBallManager.Entries[idx]);
            }

            if (isBaseIvEnabled)
            {
                BindingHelper.BindObjectToControls(grpExtraData, _baseIvManager.Entries[idx]);
            }

            _isUpdatingUI = false;
            _stateManager.SetInitialValues();
        }

        private void cmbClassIdx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUI) return;

            int newIndex = cmbClassIdx.SelectedIndex;
            if (newIndex == _currentClassIdx) return;

            if (btnSave.Enabled)
            {
                ControlHelper.HandleUnsavedChanges(
                    saveAction: () =>
                    {
                        SaveCurrentData(_currentClassIdx);
                        LoadDataToUI(newIndex);
                    },
                    discardAction: () =>
                    {
                        // restore
                        cmbClassIdx.Items[_currentClassIdx] = 
                            _nameManager.Entries[_currentClassIdx]._ClassName;
                        LoadDataToUI(newIndex);
                    },
                    cancelAction: () =>
                    {
                        cmbClassIdx.SelectedIndex = _currentClassIdx;
                    });
            }
            else
            {
                LoadDataToUI(newIndex);
            }
        }

        private void txtClassName_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUI) return;
            if (!_config.TryReadValue("ClassNameEntryLength", out int nameEntryLength)) return;

            // calc current
            int maxBytes = nameEntryLength - 1;
            string currentText = txtClassName.Text;
            byte[] currentBytes = _charmap.StringToBytes(currentText, false);

            if (currentBytes.Length > maxBytes)
            {
                while (currentText.Length > 0)
                {
                    currentBytes = _charmap.StringToBytes(currentText, false);
                    if (currentBytes.Length <= maxBytes) break;

                    currentText = currentText.Substring(0, currentText.Length - 1);
                }

                int selectionStart = txtClassName.SelectionStart;
                txtClassName.Text = currentText;
                txtClassName.SelectionStart = Math.Min(selectionStart, currentText.Length);
            }

            string validName = _charmap.BytesToString(currentBytes, 0, currentBytes.Length);
            cmbClassIdx.Items[_currentClassIdx] = validName;
        }

        private void SaveCurrentData(int idx)
        {
            // class name
            _nameManager.Entries[idx]._ClassName = txtClassName.Text;
            _nameManager.Save(idx);

            // prize multi, prizeEntryIdx != _currentClassIdx
            var prizeEntry = _prizeMultiManager.Entries.FirstOrDefault(e => e._ClassIdx == idx);
            int prizeEntryIdx = -1;

            if (prizeEntry != null)
            {
                prizeEntryIdx = _prizeMultiManager.Entries.IndexOf(prizeEntry);
            }
            else
            {
                prizeEntry = _prizeMultiManager.Entries.FirstOrDefault(e => e._ClassIdx == 0xFF);
                prizeEntryIdx = _prizeMultiManager.Entries.IndexOf(prizeEntry);
            }

            prizeEntry._ClassPrizeMulti = (byte)nudClassPrizeMulti.Value;
            _prizeMultiManager.Save(prizeEntryIdx);

            // extra data
            if (isEncounterMusicEnabled)
            {
                BindingHelper.BindControlsToObject(grpExtraData, _encounterMusicManager.Entries[idx]);
                _encounterMusicManager.Save(idx);
            }

            if (isBattleMusicEnabled)
            {
                BindingHelper.BindControlsToObject(grpExtraData, _battleMusicManager.Entries[idx]);
                _battleMusicManager.Save(idx);
            }

            if (isPokeBallEnabled)
            {
                BindingHelper.BindControlsToObject(grpExtraData, _pokeBallManager.Entries[idx]);
                _pokeBallManager.Save(idx);
            }

            if (isBaseIvEnabled)
            {
                BindingHelper.BindControlsToObject(grpExtraData, _baseIvManager.Entries[idx]);
                _baseIvManager.Save(idx);
            }

            _saveAction.Invoke();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveCurrentData(_currentClassIdx);
            _stateManager.SetInitialValues();
        }

        private void TrainerClassEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSave.Enabled)
            {
                ControlHelper.HandleUnsavedChanges(
                    () =>
                    {
                        SaveCurrentData(_currentClassIdx);
                    },
                    () =>
                    {
                        //
                    },
                    () =>
                    {
                        e.Cancel = true;
                    }
                );
            }
        }
    }
}
