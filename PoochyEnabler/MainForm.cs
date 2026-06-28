using System;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using PoochyEnabler.FileReaders;
using PoochyEnabler.Helpers;
using PoochyEnabler.Managers;

namespace PoochyEnabler
{
    public partial class MainForm : Form
    {
        private byte[] _romData = Array.Empty<byte>();
        private bool _isRomLoaded = false;
        private string _romPath = string.Empty;
        private string _iniFolder = Path.Combine(Application.StartupPath, "ini");
        private string _tblPath = Path.Combine(Application.StartupPath, "charmap.tbl");

        private IniFileReader _config;
        private TblFileReader _charmap;
        private Form _editorForm;
        private ReservationManager _reservationManager;
        private StateManager _stateManager;

        private const string ButtonPrefix = "btn";
        private const string EditorSuffix = "Editor";
        private const string FormsFolderName = "Forms";

        public MainForm()
        {
            InitializeComponent();
            InitializeControls();
            InitializeUIStates();
            InitializeEventHandlers();

            _config = new IniFileReader(_iniFolder, cmbProfile);
            _charmap = new TblFileReader(_tblPath);
            _reservationManager = new ReservationManager();
            _stateManager = new StateManager();
        }

        private void InitializeControls()
        {
            ControlHelper.AttachOffsetAutoFormat((txtStartOffset, true));
        }

        private void InitializeUIStates()
        {
            ControlHelper.ResetControls(this);
            MainFormUIUpdate();
        }

        private void MainFormUIUpdate()
        {
            bool isEditorOpen = _editorForm != null && !_editorForm.IsDisposed;

            // When ROM is not loaded AND editor is not open
            bool canLoadConfig = !_isRomLoaded && !isEditorOpen;
            btnLoadData.Enabled = canLoadConfig;
            cmbProfile.Enabled = canLoadConfig;

            // When ROM is loaded AND editor is not open
            bool canOpenEditor = _isRomLoaded && !isEditorOpen;
            btnUnloadData.Enabled = canOpenEditor;
            ControlHelper.SetControlsEnabled(grpEditors, canOpenEditor);

            // Free space finder when ROM is loaded
            ControlHelper.SetControlsEnabled(grpFreeSpaceFinder, _isRomLoaded);
        }

        private void InitializeEventHandlers()
        {
            btnLoadData.Click += btnLoadData_Click;
            btnUnloadData.Click += btnUnloadData_Click;

            // editors
            foreach (Button btn in grpEditors.Controls)
            {
                btn.Click += EditorButton_Click;
            }
            this.FormClosing += MainForm_FormClosing;

            // free space finder
            btnSearch.Click += btnSearch_Click;
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = Constants.RomFileFilter;
                openFileDialog.Title = Constants.RomFileTitle;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _romPath = openFileDialog.FileName;
                    _romData = File.ReadAllBytes(_romPath);

                    // load _config
                    string selectedConfig = cmbProfile.SelectedItem?.ToString() ?? string.Empty;
                    _config.LoadConfig(selectedConfig, _romData);

                    // txtStartOffset
                    txtStartOffset.Text = _config.TryReadValue("FreeSpaceFinderOffset", out int offsetValue)
                        ? offsetValue.ToString("X8")
                        : string.Empty;

                    // nudRequiredSize
                    nudRequiredSize.Value = _config.TryReadValue("FreeSpaceByteAmount", out int sizeValue)
                        ? sizeValue
                        : Math.Max(nudRequiredSize.Minimum, 0);

                    _isRomLoaded = true;
                    MainFormUIUpdate();
                }
            }
        }

        private void btnUnloadData_Click(object sender, EventArgs e)
        {
            _romData = Array.Empty<byte>();
            _isRomLoaded = false;
            _romPath = string.Empty;

            // clear UI
            ControlHelper.ResetControls(this);
            MainFormUIUpdate();

            // reset _config
            _config = new IniFileReader(_iniFolder, cmbProfile);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(
                txtStartOffset.Text,
                NumberStyles.HexNumber,
                null, 
                out int startOffset)) return;
            int neededBytes = (int)nudRequiredSize.Value;
            int currentOffset = (int)(((uint)startOffset + sizeof(uint) - 1) & Constants.AlignMask);
            int foundOffset = -1;

            // sorting
            var reservedInfos = _reservationManager.GetAllReservations()
                .OrderBy(res => res.Offset)
                .ToList();

            // search
            while (currentOffset + neededBytes <= _romData.Length)
            {
                bool isFreeSpace = true;

                foreach (var res in reservedInfos)
                {
                    int resStart = res.Offset;
                    int dataLength = _stateManager?.GetBinaryLength(res.StateKey) ?? 0;
                    int resEnd = resStart + dataLength;

                    int curStart = currentOffset;
                    int curEnd = currentOffset + neededBytes;

                    // no further
                    if (resStart >= curEnd)
                    {
                        break;
                    }

                    // overlap check
                    if (curStart < resEnd && curEnd > resStart)
                    {
                        isFreeSpace = false;
                        currentOffset = (int)(((uint)resEnd + sizeof(uint) - 1) & Constants.AlignMask);
                        break;
                    }
                }

                // 0xFF check
                if (isFreeSpace)
                {
                    for (int i = 0; i < neededBytes; i++)
                    {
                        if (_romData[currentOffset + i] != Constants.FreeSpaceByte)
                        {
                            isFreeSpace = false;
                            currentOffset = (int)(((uint)(currentOffset + i + sizeof(uint))) & Constants.AlignMask);
                            break;
                        }
                    }
                }

                // found?
                if (isFreeSpace)
                {
                    foundOffset = currentOffset;
                    break;
                }
            }

            // show result
            if (foundOffset != -1)
            {
                txtResult.Text = foundOffset.ToString("X8");
            }
            else
            {
                MessageBox.Show("Not found.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtResult.Text = string.Empty;
            }
        }

        private void EditorButton_Click(object sender, EventArgs e)
        {
            if (!(sender is Button btn)) return;

            // example: btnPokemon -> Pokemon -> PokemonEditor
            string editorName = btn.Name.Substring(ButtonPrefix.Length);
            string targetClassName = $"{editorName}{EditorSuffix}";

            // searching
            string baseNamespace = GetType().Namespace ?? string.Empty;
            string targetNamespace = $"{baseNamespace}.{FormsFolderName}";

            Type formType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == targetClassName && t.Namespace == targetNamespace);

            if (formType != null)
            {
                if (Activator.CreateInstance(
                    formType,
                    _romData,
                    _config,
                    _charmap,
                    _reservationManager,
                    _stateManager,
                    (Action)WriteRomToFile) is Form newForm)
                {
                    _editorForm = newForm;
                    _editorForm.FormClosed += EditorForm_FormClosed;
                    _editorForm.Show();
                    MainFormUIUpdate();
                }
            }
        }

        private void WriteRomToFile()
        {
            if (string.IsNullOrEmpty(_romPath)) return;
            File.WriteAllBytes(_romPath, _romData);
        }

        private void EditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_editorForm != null)
            {
                _editorForm.FormClosed -= EditorForm_FormClosed;
                _editorForm = null;
            }

            _reservationManager.ClearAllReservations();
            _stateManager = new StateManager();
            MainFormUIUpdate();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_editorForm != null && !_editorForm.IsDisposed)
            {
                e.Cancel = true;
                MessageBox.Show("Please close the editor.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
