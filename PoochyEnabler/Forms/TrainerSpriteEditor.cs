using System;
using System.Drawing;
using System.Windows.Forms;

using PoochyEnabler.FileReaders;
using PoochyEnabler.Helpers;
using PoochyEnabler.Managers;

namespace PoochyEnabler.Forms
{
    public partial class TrainerSpriteEditor : Form
    {
        private readonly byte[] _romData = null;
        private readonly IniFileReader _config = null;
        private readonly TblFileReader _charmap = null;
        private readonly ReservationManager _reservationManager = null;
        private readonly Action _saveAction = null;

        private StateManager _stateManager = null;

        private EntryManager<TrainerImageEntry> _imageManager = null;
        private EntryManager<TrainerPaletteEntry> _paletteManager = null;
        private EntryManager<TrainerYOffsetEntry> _yPositionManager = null;
        private EntryManager<TrainerAnimPointerEntry> _animPointerManager = null;

        private bool _isUpdatingUI = false;
        private int _currentSpriteIdx = 0;

        public TrainerSpriteEditor(
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
            InitializeEventHandlers();

            LoadDataToUI(_currentSpriteIdx);
        }

        private void InitializeManagers()
        {
            _imageManager = new EntryManager<TrainerImageEntry>(_romData, _config, _charmap);
            _imageManager.Load("TrainerImageTableOffset", "TrainerSpriteCount");
            _paletteManager = new EntryManager<TrainerPaletteEntry>(_romData, _config, _charmap);
            _paletteManager.Load("TrainerPaletteTableOffset", "TrainerSpriteCount");
            _yPositionManager = new EntryManager<TrainerYOffsetEntry>(_romData, _config, _charmap);
            _yPositionManager.Load("TrainerYPositionTableOffset", "TrainerSpriteCount");
            _animPointerManager = new EntryManager<TrainerAnimPointerEntry>(_romData, _config, _charmap);
            _animPointerManager.Load("TrainerAnimPointerTableOffset", "TrainerSpriteCount");

            _stateManager = new StateManager(hasChanges => btnSave.Enabled = hasChanges);
            _stateManager.AddControls(
                txtImageOffset,
                txtPaletteOffset,
                nudYPosition);
        }

        private void InitializeControls()
        {
            if (_config.TryReadValue("TrainerSpriteCount", out int spriteCount))
            {
                nudSpriteIdx.Maximum = Math.Min(nudSpriteIdx.Maximum, spriteCount - 1);
            }

            ControlHelper.AttachOffsetAutoFormat(txtImageOffset, txtPaletteOffset);
            ControlHelper.AttachExternalBorder(picSprite);
            ControlHelper.AttachNumericUpDownNavigators(nudSpriteIdx, btnSpriteIdxPrev, btnSpriteIdxNext);
        }

        private void InitializeEventHandlers()
        {
            /*
            btnSave.Click += btnSave_Click;
            this.FormClosing += TrainerSpriteEditor_FormClosing;

            nudSpriteIdx.ValueChanged += nudSpriteIdx_ValueChanged;
            txtImageOffset.TextChanged += SpriteOffset_Changed;
            txtPaletteOffset.TextChanged += SpriteOffset_Changed;
            btnImportImage.Click += SpriteImport_Click;
            btnImportImage.Click += SpriteImport_Click;
            btnDump.Click += btnDump_Click;
            */
        }

        private void LoadDataToUI(int idx)
        {
            _isUpdatingUI = true;
            _reservationManager.ClearAllReservations();

            _currentSpriteIdx = idx;

            BindingHelper.BindObjectToControls(this, _imageManager.Entries[idx]);
            BindingHelper.BindObjectToControls(this, _paletteManager.Entries[idx]);
            BindingHelper.BindObjectToControls(this, _yPositionManager.Entries[idx]);
            BindingHelper.BindObjectToControls(grpAnim, _animPointerManager.Entries[idx]);

            // txtAnimDataOffset
            int ptrOffset = (int)(_animPointerManager.Entries[idx].pAnimPointer - Constants.BaseAddr);
            if (IOHelper.TryReadGbaPointer(ptrOffset, _romData, out int dataOffset))
            {
                if (dataOffset == -1)
                {
                    txtAnimDataOffset.Text = "null";
                }
                else
                {
                    txtAnimDataOffset.Text = dataOffset.ToString("X8");
                }
            }

            DisplayTrainerSprite();

            _isUpdatingUI = false;
            _stateManager.SetInitialValues();
        }
    }
}
