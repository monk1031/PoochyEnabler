using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace PoochyEnabler.Helpers
{
    public static class ControlHelper
    {
        // recursive control enable/disable
        // var excludeNames = new[] { "btnTest1", "btnTest2" };
        // var excludeTypes = new[] { typeof(TextBox), typeof(ComboBox) };
        public static void SetControlsEnabled(
            this Control container,
            bool enabled,
            IEnumerable<string> excludeNames = null,
            IEnumerable<Type> excludeTypes = null)
        {
            var nameSet =
                excludeNames != null
                ? new HashSet<string>(excludeNames)
                : null;
            var typeSet =
                excludeTypes != null
                ? new HashSet<Type>(excludeTypes)
                : null;

            ExecuteRecursive(container, nameSet, typeSet, ctrl => ctrl.Enabled = enabled);
        }

        // recursive control initialization
        public static void ResetControls(
                this Control container,
                IEnumerable<string> excludeNames = null,
                IEnumerable<Type> excludeTypes = null)
        {
            var nameSet =
                excludeNames != null
                ? new HashSet<string>(excludeNames)
                : null;
            var typeSet =
                excludeTypes != null
                ? new HashSet<Type>(excludeTypes)
                : null;

            ExecuteRecursive(container, nameSet, typeSet, InitializeControl);
        }

        public static void InitializeControl(this Control ctrl)
        {
            switch (ctrl)
            {
                case TextBox textBox:
                    textBox.Text = string.Empty;
                    break;
                case NumericUpDown nud:
                    nud.Value = Math.Max(nud.Minimum, 0);
                    break;
                case ComboBox comboBox:
                    comboBox.SelectedIndex = -1;
                    break;
                case CheckBox checkBox:
                    checkBox.Checked = false;
                    break;
                case RadioButton radioButton:
                    radioButton.Checked = false;
                    break;
            }
        }

        private static void ExecuteRecursive(
                    Control ctrl,
                    HashSet<string> nameSet,
                    HashSet<Type> typeSet,
                    Action<Control> action)
        {
            if (ctrl == null) return;

            if ((nameSet?.Contains(ctrl.Name) != true) && (typeSet?.Contains(ctrl.GetType()) != true))
            {
                action(ctrl);
            }

            if (ShouldRecurse(ctrl))
            {
                foreach (Control child in ctrl.Controls)
                {
                    ExecuteRecursive(child, nameSet, typeSet, action);
                }
            }
        }

        private static bool ShouldRecurse(Control ctrl)
        {
            return ctrl is Form || ctrl is Panel || ctrl is GroupBox || ctrl is TabControl || ctrl is TabPage;
        }

        // string offset -> int offset
        public static bool TryParseOffset(string offsetStr, out int offsetValue)
        {
            return int.TryParse(offsetStr, NumberStyles.HexNumber, null, out offsetValue);
        }

        // for textbox, validating, formatting
        public static bool ValidateAndFormatInputTextBox(
            TextBox txt, 
            out int offsetValue,
            bool showMessage = true)
        {
            string offsetStr = txt.Text.Trim();

            if (!TryParseOffset(offsetStr, out int resultValue))
            {
                if (showMessage)
                {
                    MessageBox.Show(
                        "Enter a hexadecimal offset.",
                        "",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }

                offsetValue = unchecked((int)uint.MaxValue);
                return false;
            }

            offsetValue = resultValue;
            txt.Text = resultValue.ToString("X8");
            return true;
        }

        // for textbox, only formatting
        public static void AttachOffsetAutoFormat(params TextBox[] textboxes)
        {
            foreach (TextBox textbox in textboxes)
            {
                textbox.Leave -= OffsetTextBox_Leave;
                textbox.Leave += OffsetTextBox_Leave;
            }
        }

        private static void OffsetTextBox_Leave(object sender, EventArgs e)
        {
            if (!(sender is TextBox txt)) return;
            if (string.IsNullOrWhiteSpace(txt.Text)) return;

            ValidateAndFormatInputTextBox(txt, out _, showMessage: false);
        }

        // outer border
        public static void AttachExternalBorder(params Control[] targets)
        {
            foreach (Control target in targets)
            {
                Control parent = target.Parent;
                if (parent == null) continue;

                parent.Paint += (sender, e) =>
                {
                    using (var pen = new Pen(Color.Gray, 1))
                    {
                        var rect = new Rectangle(
                            target.Left - 1,
                            target.Top - 1,
                            target.Width + 1,
                            target.Height + 1);
                        e.Graphics.DrawRectangle(pen, rect);
                    }
                };
            }
        }

        // nud button navigators
        public static void AttachNumericUpDownNavigators(
            NumericUpDown nud,
            Button btnPrev,
            Button btnNext)
        {
            if (btnPrev != null)
            {
                btnPrev.Click += (sender, e) =>
                {
                    if (nud.Value > nud.Minimum)
                    {
                        nud.Value--;
                    }
                };
            }

            if (btnNext != null)
            {
                btnNext.Click += (sender, e) =>
                {
                    if (nud.Value < nud.Maximum)
                    {
                        nud.Value++;
                    }
                };
            }

            nud.ValueChanged += (sender, e) => UpdateNumericUpDownNavigators(nud, btnPrev, btnNext);
            UpdateNumericUpDownNavigators(nud, btnPrev, btnNext);
        }

        // navigator state update
        public static void UpdateNumericUpDownNavigators(
            NumericUpDown nud,
            Button btnPrev,
            Button btnNext)
        {
            if (btnPrev != null)
            {
                bool canGoPrev = nud.Value > nud.Minimum;
                if (!canGoPrev && btnPrev.Focused)
                {
                    nud.Focus();
                }
                btnPrev.Enabled = canGoPrev;
            }

            if (btnNext != null)
            {
                bool canGoNext = nud.Value < nud.Maximum;
                if (!canGoNext && btnNext.Focused)
                {
                    nud.Focus();
                }
                btnNext.Enabled = canGoNext;
            }
        }

        // radioButton linkage
        public static void AttachEnterEvent(RadioButton rb, Control ctrl)
        {
            ctrl.Enter += (sender, e) => rb.Checked = true;
        }

        // cmb setup
        public static void SetupComboBoxItems(
            ComboBox comboBox,
            int defaultIndex,
            params string[] items)
        {
            comboBox.BeginUpdate();
            try
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(items);
                comboBox.SelectedIndex = defaultIndex;
            }
            finally
            {
                comboBox.EndUpdate();
            }
        }

        // load cmb from text file
        public static void LoadComboBoxFromTextFile(ComboBox comboBox, string filePath)
        {
            var entries = new List<KeyValuePair<int, string>>();
            foreach (string line in File.ReadLines(filePath))
            {
                if (!string.IsNullOrWhiteSpace(line) && TryParseLine(line, out var entry))
                {
                    entries.Add(entry);
                }
            }

            comboBox.DisplayMember = nameof(KeyValuePair<int, string>.Value);
            comboBox.ValueMember = nameof(KeyValuePair<int, string>.Key);
            comboBox.DataSource = entries;

            bool TryParseLine(string line, out KeyValuePair<int, string> entry)
            {
                int closeBracket = line.IndexOf(']');
                if (line.StartsWith("[") && closeBracket > 1)
                {
                    string hex = line.Substring(1, closeBracket - 1);
                    if (int.TryParse(hex, NumberStyles.HexNumber, null, out int index))
                    {
                        entry = new KeyValuePair<int, string>(index, line.Trim());
                        return true;
                    }
                }

                entry = default(KeyValuePair<int, string>);
                return false;
            }
        }

        // confirm unsaved changes
        public static DialogResult HandleUnsavedChanges(
            Action saveAction,
            Action discardAction,
            Action cancelAction = null)
        {
            DialogResult result = MessageBox.Show(
                "Changes have not been saved. Save now?",
                "",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);

            switch (result)
            {
                case DialogResult.Yes:
                    saveAction?.Invoke();
                    break;
                case DialogResult.No:
                    discardAction?.Invoke();
                    break;
                case DialogResult.Cancel:
                    cancelAction?.Invoke();
                    break;
            }

            return result;
        }
    }
}
