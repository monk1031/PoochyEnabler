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
            foreach (var field in typeof(T).GetFields())
            {
                // skip
                if (field.Name.StartsWith("_")) continue;

                object value = field.GetValue(data);
                char prefix = field.Name[0];
                string baseName = field.Name.Substring(1);

                if (prefix == 'p') // ptr (subtract base aadr)
                {
                    uint ptrValue = (uint)value;
                    int offset = 
                        ptrValue == 0u
                        ? Constants.InvalidOffset
                        : (int)(ptrValue - Constants.BaseAddr);
                    SetControlValueByName(container, baseName, offset); // int
                }
                else if (prefix == 's') // signed
                {
                    if (field.FieldType == typeof(byte))
                    {
                        value = (sbyte)(byte)value;
                    }
                    else if (field.FieldType == typeof(ushort))
                    {
                        value = (short)(ushort)value;
                    }
                    else
                    {
                        value = (int)(uint)value;
                    }

                    SetControlValueByName(container, baseName, value); // object(sbyte, short, int)
                }
                else if (prefix == 'n') // nibble
                {
                    var attr = field.GetCustomAttribute<NibbleControlNamesAttribute>();
                    byte byteVal = (byte)value; // only byte

                    SetControlValueByName(
                        container, 
                        attr.HighNibbleName, 
                        (byteVal >> Constants.NibbleShift) & Constants.NibbleMask); // byte
                    SetControlValueByName(
                        container, 
                        attr.LowNibbleName,
                        byteVal & Constants.NibbleMask); // byte
                }
                else if (prefix == 'b') // bit
                {
                    var attr = field.GetCustomAttribute<BitControlNamesAttribute>();
                    byte byteVal = (byte)value; // only byte

                    for (int i = 0; i < attr.BitNames.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(attr.BitNames[i]))
                        {
                            bool isSet = ((byteVal >> i) & 1) == 1; 
                            SetControlValueByName(container, attr.BitNames[i], isSet); // bool
                        }
                    }
                }
                else // normal
                {
                    SetControlValueByName(container, field.Name, value); // object(byte, ushort, uint)
                }
            }
        }

        // UI -> structure
        public static void BindControlsToObject<T>(Control container, T data)
        {
            foreach (var field in typeof(T).GetFields())
            {
                // skip
                if (field.Name.StartsWith("_")) continue;

                char prefix = field.Name[0];
                string baseName = field.Name.Substring(1);

                if (prefix == 'p') // ptr (add base addr)
                {
                    int offset = (int)GetControlValueByName(container, baseName);
                    bool isInvalid = 
                        offset == Constants.InvalidOffset;

                    uint value = isInvalid
                        ? 0u
                        : (uint)(offset + Constants.BaseAddr);

                    field.SetValue(data, value);
                }
                else if (prefix == 's') // signed
                {
                    // null?
                    int intVal = Convert.ToInt32(GetControlValueByName(container, baseName));
                    if (field.FieldType == typeof(byte))
                    {
                        field.SetValue(data, unchecked((byte)(sbyte)intVal));
                    }
                    else if (field.FieldType == typeof(ushort))
                    {
                        field.SetValue(data, unchecked((ushort)(short)intVal));
                    }
                    else
                    {
                        field.SetValue(data, unchecked((uint)intVal));
                    }
                }
                else if (prefix == 'n') // nibble
                {
                    var attr = field.GetCustomAttribute<NibbleControlNamesAttribute>();

                    // control name = attribute name
                    int high = Convert.ToInt32(GetControlValueByName(container, attr.HighNibbleName)); 
                    int low = Convert.ToInt32(GetControlValueByName(container, attr.LowNibbleName));

                    // byte
                    field.SetValue(data, (byte)(((high & Constants.NibbleMask) << Constants.NibbleShift) | (low & Constants.NibbleMask)));
                }
                else if (prefix == 'b') // bit
                {
                    var attr = field.GetCustomAttribute<BitControlNamesAttribute>();
                    byte currentVal = Convert.ToByte(field.GetValue(data));

                    for (int i = 0; i < attr.BitNames.Length; i++)
                    {
                        if (string.IsNullOrEmpty(attr.BitNames[i])) continue;

                        bool bitVal = (bool)GetControlValueByName(container, attr.BitNames[i]);
                        byte mask = (byte)(1 << i);

                        if (bitVal)
                        {
                            currentVal |= mask;
                        }
                        else
                        {
                            currentVal &= (byte)~mask;
                        }
                    }

                    field.SetValue(data, currentVal);
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
                case TextBox txt: // due to C#7.3
                    if (value is int i)
                    {
                        txt.Text =
                            i == Constants.InvalidOffset 
                            ? "null" 
                            : i.ToString("X8");
                    }
                    else if (value is uint ui)
                    {
                        txt.Text = ui.ToString("X8");
                    }
                    else if (value is short s)
                    {
                        txt.Text = ((ushort)s).ToString("X4");
                    }
                    else if (value is ushort us)
                    {
                        txt.Text = us.ToString("X4");
                    }
                    else if (value is byte b)
                    {
                        txt.Text = b.ToString("X2");
                    }
                    else if (value is sbyte sb)
                    {
                        txt.Text = ((byte)sb).ToString("X2");
                    }
                    else // other
                    {
                        txt.Text = value?.ToString() ?? string.Empty;
                    }
                    break;
                case ComboBox cmb:
                    if (!string.IsNullOrEmpty(cmb.ValueMember))
                    {
                        cmb.SelectedValue = value; // be careful
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
                    return (int)nud.Value; // cast to int
                case TextBox txt:
                    string text = txt.Text.Trim();

                    if (string.IsNullOrEmpty(text) || text == "null")
                    {
                        return Constants.InvalidOffset;
                    }

                    if (ControlHelper.TryParseOffset(text, out int offset))
                    {
                        return offset;
                    }

                    return null;
                case ComboBox cmb:
                    return !string.IsNullOrEmpty(cmb.ValueMember)
                        ? cmb.SelectedValue
                        : cmb.SelectedIndex;
                case CheckBox chk:
                    return chk.Checked; // bool
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
