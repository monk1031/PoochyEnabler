using System;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace PoochyEnabler.Helpers
{
    public static class BindingHelper
    {
        private static readonly string[] ControlPrefixes = { "txt", "nud", "cmb", "chk" };

        // structure -> UI
        public static void BindObjectToControls<T>(Control container, T data)
        {
            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.Name.StartsWith("_")) continue;

                object value = field.GetValue(data);
                char prefix = field.Name[0];
                string baseName = field.Name.Substring(1);

                if (prefix == 'p') // pointer (subtract base address)
                {
                    uint ptrValue = (uint)value;
                    int offset = 
                        ptrValue == 0 
                        ? unchecked((int)uint.MaxValue)
                        : (int)(ptrValue - Constants.BaseAddr);
                    SetControlValueByName(container, baseName, offset);
                }
                else if (prefix == 's') // signed
                {
                    int signedValue = field.FieldType == typeof(byte) ? (sbyte)(byte)value :
                                      field.FieldType == typeof(ushort) ? (short)(ushort)value :
                                      (int)(uint)value;
                    SetControlValueByName(container, baseName, signedValue);
                }
                else if (prefix == 'n') // nibble
                {
                    var attr = field.GetCustomAttribute<NibbleControlNamesAttribute>();
                    byte byteVal = (byte)value;
                    SetControlValueByName(
                        container, 
                        attr.HighNibbleName, 
                        (byteVal >> Constants.NibbleShift) & Constants.NibbleMask);
                    SetControlValueByName(
                        container, 
                        attr.LowNibbleName, 
                        byteVal & Constants.NibbleMask);
                }
                else if (prefix == 'b') // bit
                {
                    var attr = field.GetCustomAttribute<BitControlNamesAttribute>();
                    uint uintValue = Convert.ToUInt32(value);
                    for (int i = 0; i < attr.BitNames.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(attr.BitNames[i]))
                        {
                            SetControlValueByName(container, attr.BitNames[i], ((uintValue >> i) & 1) == 1);
                        }
                    }
                }
                else
                {
                    SetControlValueByName(container, field.Name, value);
                }
            }
        }

        // UI -> structure
        public static void BindControlsToObject<T>(Control container, T data)
        {
            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.Name.StartsWith("_")) continue;

                char prefix = field.Name[0];
                string baseName = field.Name.Substring(1);

                if (prefix == 'p') // pointer (add base address)
                {
                    int offset = (int)GetControlValueByName(container, baseName);
                    field.SetValue(data, 
                        offset == unchecked((int)uint.MaxValue)
                        ? 0u 
                        : (uint)(offset + Constants.BaseAddr));
                }
                else if (prefix == 's') // signed
                {
                    int intVal = Convert.ToInt32(GetControlValueByName(container, baseName));

                    if (field.FieldType == typeof(byte))
                    {
                        field.SetValue(data, (byte)(sbyte)intVal);
                    }
                    else if (field.FieldType == typeof(ushort))
                    {
                        field.SetValue(data, (ushort)(short)intVal);
                    }
                    else
                    {
                        field.SetValue(data, (uint)intVal);
                    }
                }
                else if (prefix == 'n') // nibble
                {
                    var attr = field.GetCustomAttribute<NibbleControlNamesAttribute>();

                    int high = Convert.ToInt32(GetControlValueByName(container, attr.HighNibbleName));
                    int low = Convert.ToInt32(GetControlValueByName(container, attr.LowNibbleName));

                    field.SetValue(data, (byte)(((high & Constants.NibbleMask) << Constants.NibbleShift) | (low & Constants.NibbleMask)));
                }
                else if (prefix == 'b') // bit
                {
                    var attr = field.GetCustomAttribute<BitControlNamesAttribute>();
                    ulong currentVal = Convert.ToUInt64(field.GetValue(data)); // preserve undefined bits

                    for (int i = 0; i < attr.BitNames.Length; i++)
                    {
                        if (string.IsNullOrEmpty(attr.BitNames[i])) continue;

                        bool bitVal = (bool)GetControlValueByName(container, attr.BitNames[i]);
                        if (bitVal) currentVal |= (1UL << i);
                        else currentVal &= ~(1UL << i);
                    }

                    if (field.FieldType == typeof(byte))
                    {
                        field.SetValue(data, (byte)currentVal);
                    }
                    else if (field.FieldType == typeof(ushort))
                    {
                        field.SetValue(data, (ushort)currentVal);
                    }
                    else
                    {
                        field.SetValue(data, (uint)currentVal);
                    }
                }
                else
                {
                    object val = GetControlValueByName(container, field.Name);
                    field.SetValue(data, Convert.ChangeType(val, field.FieldType));
                }
            }
        }

        private static void SetControlValueByName(Control container, string baseName, object value)
        {
            Control ctrl = FindControl(container, baseName);

            switch (ctrl)
            {
                case NumericUpDown nud:
                    nud.Value = Convert.ToDecimal(value);
                    break;
                case TextBox txt:
                    txt.Text = value is int intVal
                        ? (intVal == unchecked((int)uint.MaxValue) 
                            ? "null" 
                            : intVal.ToString("X8"))
                        : value.ToString();
                    break;
                case ComboBox cmb:
                    if (!string.IsNullOrEmpty(cmb.ValueMember))
                    {
                        cmb.SelectedValue = value;
                    }
                    else
                    {
                        cmb.SelectedIndex = Convert.ToInt32(value);
                    }
                    break;
                case CheckBox chk:
                    chk.Checked = Convert.ToBoolean(value);
                    break;
            }
        }

        private static object GetControlValueByName(Control container, string baseName)
        {
            Control ctrl = FindControl(container, baseName);

            switch (ctrl)
            {
                case NumericUpDown nud:
                    return nud.Value;
                case TextBox txt:
                    string str = txt.Text.Trim();
                    return (str == "null" || string.IsNullOrEmpty(str))
                        ? unchecked((int)uint.MaxValue)
                        : int.Parse(str, NumberStyles.HexNumber);
                case ComboBox cmb:
                    return !string.IsNullOrEmpty(cmb.ValueMember)
                        ? cmb.SelectedValue
                        : cmb.SelectedIndex;
                case CheckBox chk:
                    return chk.Checked;
                default:
                    return null;
            }
        }

        private static Control FindControl(Control container, string baseName)
        {
            foreach (string prefix in ControlPrefixes)
            {
                var matched = container.Controls.Find(prefix + baseName, true);
                if (matched.Length > 0)
                {
                    return matched[0];
                }
            }

            return null;
        }

        [AttributeUsage(AttributeTargets.Field)]
        public class NibbleControlNamesAttribute : Attribute
        {
            public string HighNibbleName { get; }
            public string LowNibbleName { get; }

            public NibbleControlNamesAttribute(string highName, string lowName)
            {
                HighNibbleName = highName;
                LowNibbleName = lowName;
            }
        }

        [AttributeUsage(AttributeTargets.Field)]
        public class BitControlNamesAttribute : Attribute
        {
            public string[] BitNames { get; }
            public BitControlNamesAttribute(params string[] names)
            {
                BitNames = names;
            }
        }
    }
}
