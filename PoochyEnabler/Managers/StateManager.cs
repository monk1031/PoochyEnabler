using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using PoochyEnabler.Helpers;

namespace PoochyEnabler.Managers
{
    public class StateManager
    {
        private readonly Action<bool> _stateChangedCallback;
        private readonly Dictionary<Control, object> _initialControlValues = new Dictionary<Control, object>();
        private readonly Dictionary<string, DataState> _dataStates = new Dictionary<string, DataState>();
        private readonly List<RadioButtonGroup> _radioGroups = new List<RadioButtonGroup>();

        public StateManager(Action<bool> stateChangedCallback)
        {
            _stateChangedCallback = stateChangedCallback;
            _stateChangedCallback.Invoke(false);
        }

        public abstract class DataState
        {
            public abstract bool HasChanges();
            public abstract void Initialize();
        }

        public class DataState<T> : DataState // include array
        {
            public T InitialData { get; set; }
            public T CurrentData { get; set; }

            public DataState(T initialData)
            {
                InitialData = CreateClone(initialData);
                CurrentData = initialData;
            }

            public override bool HasChanges()
            {
                // both null
                if (InitialData == null && CurrentData == null) return false;

                // either null
                if (InitialData == null || CurrentData == null) return true;

                // cmp array
                if (InitialData is Array array1 && CurrentData is Array array2)
                {
                    // instant check
                    if (array1.Length != array2.Length) return true;

                    // strict check
                    return !StructuralComparisons.StructuralEqualityComparer.Equals(array1, array2);
                }

                // cmp normal
                return !EqualityComparer<T>.Default.Equals(InitialData, CurrentData);
            }

            public override void Initialize()
            {
                InitialData = CreateClone(CurrentData);
            }

            private T CreateClone(T source)
            {
                if (source == null) return default;

                if (source is ICloneable cloneable)
                {
                    return (T)cloneable.Clone();
                }

                // as it is
                return source;
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
        public void AddDatas<T>(params (string Name, T Data)[] items)
        {
            foreach (var (name, data) in items)
            {
                if (_dataStates.ContainsKey(name))
                    continue;

                _dataStates.Add(name, new DataState<T>(data));
            }
        }

        public void UpdateData<T>(string name, T newData)
        {
            // not exist
            if (!_dataStates.TryGetValue(name, out var state)) return;

            if (state is DataState<T> dataState)
            {
                dataState.CurrentData = newData;
                EvaluateState();
            }
        }

        // to write to rom
        public T GetCurrentData<T>(string name)
        {
            if (!_dataStates.TryGetValue(name, out var state)) return default;

            if (state is DataState<T> dataState)
            {
                return dataState.CurrentData;
            }

            return default;
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

            foreach (var state in _dataStates.Values)
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
                           || DetectDataChanges()
                           || DetectRadioChanges();

            _stateChangedCallback.Invoke(hasChanges);
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

        private bool DetectDataChanges()
        {
            return _dataStates.Values.Any(state => state.HasChanges());
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
