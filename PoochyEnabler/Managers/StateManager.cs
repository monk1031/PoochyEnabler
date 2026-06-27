using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using PoochyEnabler.Helpers;

namespace PoochyEnabler.Managers
{
    public class StateManager
    {
        public event Action<bool> StateChanged;

        private readonly Dictionary<Control, object> _initialControlValues = new Dictionary<Control, object>();
        private readonly Dictionary<string, BinaryState> _binaryStates = new Dictionary<string, BinaryState>();
        private readonly List<RadioButtonGroup> _radioGroups = new List<RadioButtonGroup>();

        public class BinaryState
        {
            public byte[] InitialBinary { get; set; }
            public byte[] CurrentBinary { get; set; }

            public BinaryState(byte[] initialData)
            {
                InitialBinary = initialData?.ToArray();
                CurrentBinary = initialData;
            }

            public bool HasChanges()
            {
                // both null
                if (InitialBinary == null && CurrentBinary == null) return false;

                // either null
                if (InitialBinary == null || CurrentBinary == null) return true;

                // instant check
                if (InitialBinary.Length != CurrentBinary.Length) return true;

                // strict check
                return !InitialBinary.SequenceEqual(CurrentBinary);
            }

            public void Initialize()
            {
                InitialBinary = CurrentBinary?.ToArray();
            }
        }

        // due to complexity
        private class RadioButtonGroup
        {
            public RadioButton[] Buttons { get; set; }
            public RadioButton InitialChecked { get; set; }
        }

        // register controls
        public void AddControls(params Control[] controls)
        {
            foreach (var control in controls)
            {
                if (_initialControlValues.ContainsKey(control)) continue;
                _initialControlValues.Add(control, GetControlValue(control));

                switch (control)
                {
                    case NumericUpDown nud:
                        nud.ValueChanged += (s, e) => EvaluateState();
                        break;
                    case TextBox txt:
                        txt.TextChanged += (s, e) => EvaluateState();
                        break;
                    case ComboBox cmb:
                        cmb.SelectedIndexChanged += (s, e) => EvaluateState();
                        break;
                    case CheckBox chk:
                        chk.CheckedChanged += (s, e) => EvaluateState();
                        break;
                }
            }
        }

        // for container
        public void AddControlsRecursive(params Control[] containers)
        {
            var targetControls = new List<Control>();

            foreach (var container in containers)
            {
                if (ControlHelper.ShouldRecurse(container))
                {
                    FindTargetControls(container, targetControls);
                }
            }

            if (targetControls.Count > 0)
            {
                AddControls(targetControls.ToArray());
            }
        }

        private void FindTargetControls(Control parent, List<Control> targetControls)
        {
            if (parent == null) return;

            foreach (Control child in parent.Controls)
            {
                if (IsTrackedControl(child))
                {
                    targetControls.Add(child);
                }
                else if (ControlHelper.ShouldRecurse(child))
                {
                    FindTargetControls(child, targetControls);
                }
            }
        }

        // control list
        private bool IsTrackedControl(Control ctrl)
        {
            return ctrl is NumericUpDown || ctrl is TextBox || ctrl is ComboBox || ctrl is CheckBox;
        }

        // resiter binary
        public void AddBinaries(params (string Name, byte[] Data)[] items)
        {
            foreach (var (name, data) in items)
            {
                if (_binaryStates.ContainsKey(name)) continue;
                _binaryStates.Add(name, new BinaryState(data));
            }
        }

        public void UpdateBinary(string name, byte[] newData)
        {
            // not exist
            if (!_binaryStates.TryGetValue(name, out var state)) return;

            state.CurrentBinary = newData;
            EvaluateState();
        }

        // to write to rom
        public byte[] GetCurrentBinary(string name)
        {
            return _binaryStates.TryGetValue(name, out var state)
                ? state.CurrentBinary
                : null;
        }

        // to search for free space
        public int GetBinaryLength(string name)
        {
            return _binaryStates.TryGetValue(name, out var state) && state.CurrentBinary != null
                ? state.CurrentBinary.Length
                : 0;
        }

        // ridio button group
        public void AddRadioButtons(params RadioButton[][] groups)
        {
            foreach (var group in groups)
            {
                var radioGroup = new RadioButtonGroup
                {
                    Buttons = group,
                    InitialChecked = group.FirstOrDefault(rb => rb.Checked)
                };

                _radioGroups.Add(radioGroup);

                foreach (var rb in group)
                {
                    rb.CheckedChanged += OnRadioButtonCheckedChanged;
                }
            }
        }

        private void OnRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            EvaluateState();
        }

        // initialize all
        public void SetInitialValues()
        {
            foreach (var ctrl in _initialControlValues.Keys.ToList())
            {
                _initialControlValues[ctrl] = GetControlValue(ctrl);
            }

            foreach (var state in _binaryStates.Values)
            {
                state.Initialize();
            }

            foreach (var group in _radioGroups)
            {
                group.InitialChecked = group.Buttons.FirstOrDefault(rb => rb.Checked);
            }

            EvaluateState();
        }

        private void EvaluateState()
        {
            bool hasChanges = DetectControlChanges()
                           || DetectBinaryChanges()
                           || DetectRadioChanges();

            StateChanged.Invoke(hasChanges);
        }

        private bool DetectControlChanges()
        {
            foreach (KeyValuePair<Control, object> kvp in _initialControlValues)
            {
                if (!Equals(GetControlValue(kvp.Key), kvp.Value))
                {
                    return true;
                }
            }
            return false;
        }

        private bool DetectBinaryChanges()
        {
            return _binaryStates.Values.Any(state => state.HasChanges());
        }

        private bool DetectRadioChanges()
        {
            foreach (RadioButtonGroup group in _radioGroups)
            {
                RadioButton current = group.Buttons.FirstOrDefault(rb => rb.Checked);
                if (!ReferenceEquals(current, group.InitialChecked))
                {
                    return true;
                }
            }
            return false;
        }

        private object GetControlValue(Control control)
        {
            switch (control)
            {
                case NumericUpDown nud:
                    return nud.Value;
                case TextBox txt:
                    return txt.Text;
                case ComboBox cmb:
                    return (!string.IsNullOrEmpty(cmb.ValueMember) && cmb.SelectedValue != null)
                        ? cmb.SelectedValue
                        : cmb.SelectedIndex;
                case CheckBox chk:
                    return chk.Checked;
                default:
                    return null;
            }
        }
    }
}