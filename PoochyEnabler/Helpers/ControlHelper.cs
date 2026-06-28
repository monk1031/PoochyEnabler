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
            Control container,
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

            ExecuteRecursive(
                container, 
                nameSet, 
                typeSet, 
                ctrl => 
                {
                    ctrl.Enabled = enabled;
                });
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

            ExecuteRecursive(
                container, 
                nameSet, 
                typeSet,
                ctrl => 
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
                });
        }

        private static void ExecuteRecursive(
                    Control target,
                    HashSet<string> nameSet,
                    HashSet<Type> typeSet,
                    Action<Control> action)
        {
            if (target == null) return;

            if ((nameSet?.Contains(target.Name) != true) && 
                (typeSet?.Contains(target.GetType()) != true)) // null -> false
            {
                action(target);
            }

            if (ShouldRecurse(target))
            {
                foreach (Control child in target.Controls)
                {
                    ExecuteRecursive(child, nameSet, typeSet, action);
                }
            }
        }

        public static bool ShouldRecurse(Control ctrl)
        {
            return ctrl is Form || 
                   ctrl is Panel || 
                   ctrl is GroupBox || 
                   ctrl is TabControl || 
                   ctrl is TabPage;
        }

        // textbox formatting, show message on/off
        public static void AttachOffsetAutoFormat(params (TextBox Textbox, bool ShowMessage)[] textboxes)
        {
            foreach (var (textbox, showMessage) in textboxes)
            {
                textbox.Leave += (sender, e) =>
                {
                    // guard
                    if (!(sender is TextBox txt)) return;

                    // IsNullOrWhiteSpace, "null"
                    if (string.IsNullOrWhiteSpace(txt.Text) || txt.Text.Trim().Equals("null")) return;

                    if (!int.TryParse(txt.Text.Trim(), NumberStyles.HexNumber, null, out int resultValue))
                    {
                        if (showMessage)
                        {
                            MessageBox.Show(
                                "Enter a hexadecimal offset.",
                                "",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }

                        txt.Text = string.Empty; // clear
                        return;
                    }

                    txt.Text = resultValue.ToString("X8");
                    return;
                };
            }
        }

        // draw border
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
            btnPrev.Click += (sender, e) =>
            {
                if (nud.Value > nud.Minimum)
                {
                    nud.Value--;
                }
            };

            btnNext.Click += (sender, e) =>
            {
                if (nud.Value < nud.Maximum)
                {
                    nud.Value++;
                }
            };

            nud.ValueChanged += (sender, e) =>
            {
                UpdateNumericUpDownNavigators(nud, btnPrev, btnNext);
            };

            UpdateNumericUpDownNavigators(nud, btnPrev, btnNext);
        }

        // navigator state update
        public static void UpdateNumericUpDownNavigators(
            NumericUpDown nud,
            Button btnPrev,
            Button btnNext)
        {
            bool canGoPrev = nud.Value > nud.Minimum;
            if (!canGoPrev && btnPrev.Focused)
            {
                nud.Focus();
            }
            btnPrev.Enabled = canGoPrev;

            bool canGoNext = nud.Value < nud.Maximum;
            if (!canGoNext && btnNext.Focused)
            {
                nud.Focus();
            }
            btnNext.Enabled = canGoNext;
        }

        // radioButton linkage
        public static void AttachEnterEvent(RadioButton rb, Control ctrl)
        {
            ctrl.Enter += (sender, e) =>
            {
                rb.Checked = true;
            };
        }

        // cmb setup, instant
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
                if (!string.IsNullOrWhiteSpace(line) && 
                    TryParseLine(line, out var entry))
                {
                    entries.Add(entry);
                }
            }

            comboBox.DisplayMember = nameof(KeyValuePair<int, string>.Value);
            comboBox.ValueMember = nameof(KeyValuePair<int, string>.Key);
            comboBox.DataSource = entries;

            // [XX]ABCD
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

                // fail
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
