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
        private readonly StateManager _stateManager = null;
        private readonly Action _saveAction = null;

        private EntryManager<TrainerImageEntry> _imageManager = null;
        private EntryManager<TrainerPaletteEntry> _paletteManager = null;
        private EntryManager<TrainerYOffsetEntry> _yPositionManager = null;
        private EntryManager<TrainerAnimPointerEntry> _animPointerManager = null;

        private bool _isUpdatingUI = false;
        private int _currentSpriteIdx = 0;

        private static class StateKeys
        {
            public static readonly string ImageData = nameof(ImageData);
            public static readonly string PaletteData = nameof(PaletteData);
        }

        public TrainerSpriteEditor(
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

            _stateManager.StateChanged += hasChanges => btnSave.Enabled = hasChanges;
            _stateManager.AddControls(
                txtImageOffset,
                txtPaletteOffset,
                nudYPosition);
            _stateManager.AddBinaries(
                (StateKeys.ImageData, null),
                (StateKeys.PaletteData, null));
        }

        private void InitializeControls()
        {
            nudSpriteIdx.Maximum = Math.Min(nudSpriteIdx.Maximum, _config.ReadInt("TrainerSpriteCount") - 1);

            ControlHelper.AttachOffsetAutoFormat((txtImageOffset, false), (txtPaletteOffset, false));
            ControlHelper.AttachExternalBorder(picSprite);
            ControlHelper.AttachNumericUpDownNavigators(nudSpriteIdx, btnSpriteIdxPrev, btnSpriteIdxNext);

            // set tag
            btnImportImage.Tag = StateKeys.ImageData;
            btnImportPalette.Tag = StateKeys.PaletteData;
        }

        private void InitializeEventHandlers()
        {
            btnSave.Click += btnSave_Click;
            this.FormClosing += TrainerSpriteEditor_FormClosing;

            nudSpriteIdx.ValueChanged += nudSpriteIdx_ValueChanged;
            txtImageOffset.TextChanged += SpriteOffset_TextChanged;
            txtPaletteOffset.TextChanged += SpriteOffset_TextChanged;
            btnImportImage.Click += SpriteImport_Click;
            btnImportPalette.Click += SpriteImport_Click;
            btnExport.Click += btnExport_Click;
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
            if (IOHelper.TryReadPtr(ptrOffset, _romData, out int dataOffset))
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

        private void nudSpriteIdx_ValueChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUI) return;

            int newIndex = (int)nudSpriteIdx.Value;
            if (newIndex == _currentSpriteIdx) return;

            if (btnSave.Enabled)
            {
                ControlHelper.HandleUnsavedChanges(
                    saveAction: () =>
                    {
                        SaveCurrentData(_currentSpriteIdx);
                        LoadDataToUI(newIndex);
                    },
                    discardAction: () =>
                    {
                        LoadDataToUI(newIndex);
                    },
                    cancelAction: () =>
                    {
                        nudSpriteIdx.Value = _currentSpriteIdx;
                    });
            }
            else
            {
                LoadDataToUI(newIndex);
            }
        }

        private void DisplayTrainerSprite()
        {
            bool isImageValid =
                ControlHelper.TryParseOffset(txtImageOffset.Text, out int imageOffset);
            bool isPaletteValid =
                ControlHelper.TryParseOffset(txtPaletteOffset.Text, out int paletteOffset);

            if (!isImageValid || !isPaletteValid)
            {
                picSprite.Image?.Dispose();
                picSprite.Image = null;
                return;
            }

            try
            {
                byte[] imageData;
                byte[] paletteData;

                // check reservation
                var imageRes = _reservationManager.GetReservation(txtImageOffset);
                if (imageRes != null)
                {
                    imageData = ImageHelper.DecompressLZ77(_stateManager.GetCurrentBinary(StateKeys.ImageData), 0);
                }
                else
                {
                    imageData = ImageHelper.DecompressLZ77(_romData, imageOffset);
                    _stateManager.UpdateBinary(StateKeys.ImageData, imageData);
                }

                // check reservation
                var PaletteRes = _reservationManager.GetReservation(txtPaletteOffset);
                if (PaletteRes != null)
                {
                    paletteData = ImageHelper.DecompressPalette(_stateManager.GetCurrentBinary(StateKeys.PaletteData), 0, true);
                }
                else
                {
                    paletteData = ImageHelper.DecompressPalette(_romData, paletteOffset, true);
                    _stateManager.UpdateBinary(StateKeys.PaletteData, paletteData);
                }

                Bitmap sprite = ImageHelper.CreateBitmap(
                    imageData,
                    paletteData,
                    Constants.SpriteSize,
                    Constants.SpriteSize,
                    true);

                picSprite.Image?.Dispose();
                picSprite.Image = sprite;
                picSprite.Refresh();
            }
            catch
            {
                picSprite.Image?.Dispose();
                picSprite.Image = null;
            }
        }

        private void SpriteOffset_TextChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUI) return;
            DisplayTrainerSprite();
        }

        private void SpriteImport_Click(object sender, EventArgs e)
        {
            if (!(sender is Button btn) || !(btn.Tag is string stateKey)) return;

            using (var popup = new QuickInputPopup(
                defaultOffset: 0,
                fileFilter: Constants.ImageImportFilter))
            {
                if (popup.ShowDialog() == DialogResult.OK)
                {
                    int offset = popup.Offset;
                    string filePath = popup.FilePath;

                    using (Bitmap bmp = new Bitmap(filePath))
                    {
                        TextBox targetTextBox;

                        if (!ImageHelper.ExtractImageAndPalette(
                            bmp,
                            Constants.SpriteSize,
                            Constants.SpriteSize,
                            out byte[] imageData,
                            out byte[] paletteData)) return;

                        if (stateKey == StateKeys.ImageData)
                        {
                            _stateManager.UpdateBinary(stateKey, ImageHelper.CompressLZ77(imageData));
                            targetTextBox = txtImageOffset;
                        }
                        else
                        {
                            _stateManager.UpdateBinary(stateKey, ImageHelper.CompressPalette(paletteData, true));
                            targetTextBox = txtPaletteOffset;
                        }

                        _reservationManager.SetReservation(targetTextBox, offset, stateKey);
                    }

                    DisplayTrainerSprite();
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (picSprite.Image == null) return;

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = Constants.ImageExportFilter;
                sfd.FileName = $"trainer_sprite_{(int)nudSpriteIdx.Value:D4}";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ImageHelper.ExportIndexedImage((Bitmap)picSprite.Image, sfd.FileName);
                }
            }
        }

        private void SaveCurrentData(int idx)
        {
            // check reservation
            var imageRes = _reservationManager.GetReservation(txtImageOffset);
            var paletteRes = _reservationManager.GetReservation(txtPaletteOffset);

            if (imageRes != null)
            {
                var imageData = _stateManager.GetCurrentBinary(imageRes.StateKey);

                if (imageData != null)
                {
                    IOHelper.WriteBytesToRom(_romData, imageRes.Offset, imageData);
                    _reservationManager.ClearReservation(txtImageOffset);
                }
            }

            if (paletteRes != null)
            {
                var paletteData = _stateManager.GetCurrentBinary(paletteRes.StateKey);

                if (paletteData != null)
                {
                    IOHelper.WriteBytesToRom(_romData, paletteRes.Offset, paletteData);
                    _reservationManager.ClearReservation(txtPaletteOffset);
                }
            }

            BindingHelper.BindControlsToObject(this, _imageManager.Entries[idx]);
            _imageManager.Save(idx);

            BindingHelper.BindControlsToObject(this, _paletteManager.Entries[idx]);
            _paletteManager.Save(idx);

            BindingHelper.BindControlsToObject(this, _yPositionManager.Entries[idx]);
            _yPositionManager.Save(idx);

            // BindingHelper.BindControlsToObject(grpAnim, _animPointerManager.Entries[idx]);
            // _animPointerManager.Save(idx);

            _saveAction.Invoke();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveCurrentData(_currentSpriteIdx);
            _stateManager.SetInitialValues();
        }

        private void TrainerSpriteEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSave.Enabled)
            {
                ControlHelper.HandleUnsavedChanges(
                    saveAction: () =>
                    {
                        SaveCurrentData(_currentSpriteIdx);
                    },
                    discardAction: () =>
                    {
                        //
                    },
                    cancelAction: () =>
                    {
                        e.Cancel = true;
                    }
                );
            }
        }
    }
}
