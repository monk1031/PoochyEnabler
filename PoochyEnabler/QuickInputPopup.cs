using System;
using System.Windows.Forms;
using PoochyEnabler.Helpers;

namespace PoochyEnabler
{
    public partial class QuickInputPopup : Form
    {
        public int Offset { get; set; }
        public int DataTypeIndex { get; set; }
        public int EntryCount { get; set; }
        public string FilePath { get; set; }

        private readonly string _fileFilter;

        public QuickInputPopup(
            int? defaultOffset = null,
            string[] comboItems = null,
            decimal? nudMin = null,
            decimal? nudMax = null,
            string fileFilter = null)
        {
            InitializeComponent();
            _fileFilter = fileFilter;

            // temporary all disabled
            ControlHelper.SetControlsEnabled(grpInput, false, false);

            SetupOffsetInput(defaultOffset);
            SetupComboBox(comboItems);
            SetupNumericUpDown(nudMin, nudMax);
            SetupFileInput(fileFilter);

            // event handler
            btnBrowse.Click += BtnBrowse_Click;
            btnApply.Click += BtnApply_Click;

            ControlHelper.AttachOffsetAutoFormat((txtTargetOffset, true));
        }

        private void SetupOffsetInput(int? offset)
        {
            if (!offset.HasValue) return;

            lblTargetOffset.Enabled = true;
            txtTargetOffset.Enabled = true;
            txtTargetOffset.Text = offset.Value.ToString("X8");
        }

        private void SetupComboBox(string[] items)
        {
            if (items == null || items.Length == 0) return;

            lblDataType.Enabled = true;
            cmbDataType.Enabled = true;
            cmbDataType.Items.Clear();
            cmbDataType.Items.AddRange(items);
            cmbDataType.SelectedIndex = 0;
        }

        private void SetupNumericUpDown(decimal? min, decimal? max)
        {
            if (!min.HasValue || !max.HasValue) return;

            lblEntryCount.Enabled = true;
            nudEntryCount.Enabled = true;
            nudEntryCount.Minimum = min.Value;
            nudEntryCount.Maximum = max.Value;
            nudEntryCount.Value = min.Value;
        }

        private void SetupFileInput(string filter)
        {
            if (string.IsNullOrEmpty(filter)) return;

            txtSelectFile.Enabled = true;
            btnBrowse.Enabled = true;
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = _fileFilter })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtSelectFile.Text = ofd.FileName;
                }
            }
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            if (!ValidateAndExtractInputs()) return; 
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateAndExtractInputs()
        {
            // check offset
            if (txtTargetOffset.Enabled)
            {
                if (!ControlHelper.TryParseOffset(txtTargetOffset.Text, out int offset))
                {
                    return false;
                }

                Offset = offset;
            }

            // check data type
            if (cmbDataType.Enabled)
            {
                DataTypeIndex = cmbDataType.SelectedIndex;
            }

            // check entry count
            if (nudEntryCount.Enabled)
            {
                EntryCount = (int)nudEntryCount.Value;
            }

            // check file path
            if (txtSelectFile.Enabled)
            {
                string path = txtSelectFile.Text.Trim();
                if (string.IsNullOrEmpty(path))
                {
                    MessageBox.Show(
                        "Select a File.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                FilePath = path;
            }

            return true;
        }
    }
}