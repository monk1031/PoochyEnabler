using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using PoochyEnabler.FileReaders;
using PoochyEnabler.Helpers;

namespace PoochyEnabler
{
    public partial class MainForm : Form
    {
        private byte[] _romData = Array.Empty<byte>();
        private bool _isRomLoaded = false;
        private string _romPath = string.Empty;
        private string _iniPath = Path.Combine(Application.StartupPath, "cfg", "roms.ini");
        private string _tblPath = Path.Combine(Application.StartupPath, "cfg", "charmap.tbl");

        private IniFileReader _config;
        private TblFileReader _charmap;
        private Form _editorForm;

        private const string ButtonPrefix = "btn";
        private const string EditorSuffix = "Editor";
        private const string FormsFolderName = "Forms";

        public MainForm()
        {
            InitializeComponent();
            InitializeUIStates();
            InitializeEventHandlers();

            _config = new IniFileReader(_iniPath, cmbProfile);
            _charmap = new TblFileReader(_tblPath);
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
            lblProfile.Enabled = canLoadConfig;
            cmbProfile.Enabled = canLoadConfig;

            // When ROM is loaded AND editor is not open
            bool canOpenEditor = _isRomLoaded && !isEditorOpen;
            btnSaveData.Enabled = canOpenEditor;
            btnUnloadData.Enabled = canOpenEditor;
            ControlHelper.SetControlsEnabled(grpEditors, canOpenEditor);

            // Free space finder when ROM is loaded
            ControlHelper.SetControlsEnabled(grpFreeSpaceFinder, _isRomLoaded);
        }

        private void InitializeEventHandlers()
        {
            btnLoadData.Click += btnLoadData_Click;
            btnSaveData.Click += btnSaveData_Click;
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

                    string selectedConfig = cmbProfile.SelectedItem?.ToString() ?? string.Empty;
                    _config.LoadConfig(selectedConfig, _romData);

                    // txtStartAddress
                    uint? rawOffset = _config.ReadOffset("FreeSpaceFinderAddress");
                    txtStartAddress.Text = rawOffset is uint offsetValue
                        ? offsetValue.ToString("X8")
                        : string.Empty;

                    // nudRequiredSize
                    nudRequiredSize.Value = _config.ReadNumber("FreeSpaceByteAmount");

                    _isRomLoaded = true;
                    MainFormUIUpdate();
                }
            }
        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = Constants.RomFileFilter;
                saveFileDialog.Title = Constants.RomFileTitle;
                saveFileDialog.FileName = Path.GetFileName(_romPath);

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveFileDialog.FileName, _romData);
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

            // reset config
            _config = new IniFileReader(_iniPath, cmbProfile);
        }

        private void EditorButton_Click(object sender, EventArgs e)
        {
            if (!(sender is Button btn)) return;

            // example: btnPokemon -> Pokemon -> PokemonEditor
            string editorName = btn.Name.Substring(ButtonPrefix.Length);
            string targetClassName = $"{editorName}{EditorSuffix}";

            // search folder
            string baseNamespace = GetType().Namespace ?? string.Empty;
            string targetNamespace = $"{baseNamespace}.{FormsFolderName}";

            Type formType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(t => t.Name == targetClassName && t.Namespace == targetNamespace);

            if (formType != null)
            {
                if (Activator.CreateInstance(formType, _romData, _config, _charmap) is Form newForm)
                {
                    OpenEditorForm(newForm);
                }
            }
        }

        private void OpenEditorForm(Form form)
        {
            if (_editorForm != null && !_editorForm.IsDisposed)
            {
                _editorForm.Focus();
                return;
            }

            _editorForm = form;
            _editorForm.FormClosed += EditorForm_FormClosed;

            _editorForm.Show();
            MainFormUIUpdate();
        }

        private void EditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_editorForm != null)
            {
                _editorForm.FormClosed -= EditorForm_FormClosed;
                _editorForm = null;
            }

            // _reservationManager.ClearAllReservations();
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
