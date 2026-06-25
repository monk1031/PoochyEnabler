using System;
using System.Reflection;
using System.Windows.Forms;

namespace PoochyEnabler.Helpers
{
    public static class BindingHelper
    {
        // radio button excluded due to complexity
        private static readonly string[] ControlPrefixes = { "txt", "nud", "cmb", "chk" };
        // prevent loop
        private static bool _isSyncing = false;

        public static void StartAutoSync(Control container, object obj)
        {
            // initialize
            BindObjectToControls(container, obj);
            // attach event
            AttachEventsRecursive(container, container, obj);
        }








        // structure -> UI, exclude string field
        public static void BindObjectToControls<T>(Control container, T data) where T : class
        {
            if (_isSyncing) return; // prevent loop
            _isSyncing = true;

            try
            {
                foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    // skip
                    if (field.Name.StartsWith("_")) continue;

                    object value = field.GetValue(data);
                    if (value == null) continue;

                    // pointer (substract base address)
                    if (field.Name.StartsWith("p"))
                    {
                        uint ptrValue = (uint)value;
                        int offset = ptrValue == 0
                            ? unchecked((int)uint.MaxValue)
                            : (int)(ptrValue - Constants.BaseAddr);
                        SetControlValueByName(container, field.Name.Substring(1), offset, true);
                    }
                    // singed
                    else if (field.Name.StartsWith("s"))
                    {
                        decimal signedValue;
                        unchecked
                        {
                            switch (Type.GetTypeCode(field.FieldType))
                            {
                                case TypeCode.Byte:
                                    signedValue = (sbyte)(byte)value;
                                    break;
                                case TypeCode.UInt16:
                                    signedValue = (short)(ushort)value;
                                    break;
                                case TypeCode.UInt32:
                                    signedValue = (int)(uint)value;
                                    break;
                                default:
                                    signedValue = 0m;
                                    break;
                            }
                        }

                        SetControlValueByName(container, field.Name.Substring(1), signedValue, false);
                    }
                    // nibble
                    else if (field.Name.StartsWith("n"))
                    {
                        var attr = field.GetCustomAttribute<NibbleControlNamesAttribute>();
                        if (value is byte byteVal)
                        {
                            // upper
                            SetControlValueByName(
                                container,
                                attr.HighNibbleName,
                                (byteVal >> Constants.NibbleShift) & Constants.NibbleMask,
                                false);

                            // lower
                            SetControlValueByName(
                                container,
                                attr.LowNibbleName,
                                byteVal & Constants.NibbleMask,
                                false);
                        }
                    }
                    // bit
                    else if (field.Name.StartsWith("b"))
                    {
                        var attr = field.GetCustomAttribute<BitControlNamesAttribute>();
                        uint uintValue = Convert.ToUInt32(value);
                        for (int i = 0; i < attr.BitNames.Length; i++)
                        {
                            string bitName = attr.BitNames[i];
                            if (!string.IsNullOrEmpty(bitName))
                            {
                                SetControlValueByName(container, bitName, ((uintValue >> i) & 1) == 1, false);
                            }
                        }
                    }
                    else
                    {
                        SetControlValueByName(container, field.Name, value, false);
                    }
                }
            }
            finally
            {
                _isSyncing = false;
            }
        }

        // UI -> structure
        public static void BindControlsToObject<T>(Control container, T data) where T : class
        {
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                // skip
                if (field.Name.StartsWith("_")) continue;

                // pointer (add base address)
                if (field.Name.StartsWith("p"))
                {
                    object value = GetControlValueByName(container, field.Name.Substring(1), typeof(int));

                    if (value is int offset)
                    {
                        field.SetValue(data, offset == unchecked((int)uint.MaxValue)
                            ? 0u
                            : (uint)(offset + Constants.BaseAddr));
                    }
                    else
                    {
                        field.SetValue(data, 0u);
                    }
                }
                // singed
                else if (field.Name.StartsWith("s"))
                {
                    object val = GetControlValueByName(container, field.Name.Substring(1), typeof(decimal));
                    if (val == null) continue;

                    int intVal = (int)Math.Truncate(Convert.ToDecimal(val));

                    unchecked
                    {
                        switch (Type.GetTypeCode(field.FieldType))
                        {
                            case TypeCode.Byte:
                                field.SetValue(data, (byte)intVal);
                                break;
                            case TypeCode.UInt16:
                                field.SetValue(data, (ushort)intVal);
                                break;
                            case TypeCode.UInt32:
                                field.SetValue(data, (uint)intVal);
                                break;
                        }
                    }
                }
                // nibble
                else if (field.Name.StartsWith("n"))
                {
                    var attr = field.GetCustomAttribute<NibbleControlNamesAttribute>();
                    if (attr == null || Type.GetTypeCode(field.FieldType) != TypeCode.Byte) continue;

                    byte current = (byte)(field.GetValue(data) ?? (byte)0);

                    object highObj = GetControlValueByName(container, attr.HighNibbleName, typeof(int));
                    object lowObj = GetControlValueByName(container, attr.LowNibbleName, typeof(int));

                    int high = highObj != null
                        ? Convert.ToInt32(highObj)
                        : (current >> Constants.NibbleShift) & Constants.NibbleMask;
                    int low = lowObj != null
                        ? Convert.ToInt32(lowObj)
                        : current & Constants.NibbleMask;

                    field.SetValue(data, (byte)(((high & Constants.NibbleMask) << Constants.NibbleShift)
                                               | (low & Constants.NibbleMask)));
                }
                // bit
                else if (field.Name.StartsWith("b"))
                {
                    var attr = field.GetCustomAttribute<BitControlNamesAttribute>();
                    if (attr == null) continue;

                    long intValue = Convert.ToInt64(field.GetValue(data) ?? 0L);

                    for (int i = 0; i < attr.BitNames.Length; i++)
                    {
                        string bitName = attr.BitNames[i];
                        if (string.IsNullOrEmpty(bitName)) continue;

                        object valObj = GetControlValueByName(container, bitName, typeof(bool));
                        if (valObj is bool bitVal)
                        {
                            if (bitVal)
                            {
                                intValue |= (1L << i);
                            }
                            else
                            {
                                intValue &= ~(1L << i);
                            }
                        }
                    }

                    unchecked
                    {
                        switch (Type.GetTypeCode(field.FieldType))
                        {
                            case TypeCode.Byte:
                                field.SetValue(data, (byte)intValue);
                                break;
                            case TypeCode.UInt16:
                                field.SetValue(data, (ushort)intValue);
                                break;
                            case TypeCode.UInt32:
                                field.SetValue(data, (uint)intValue);
                                break;
                        }
                    }
                }
                else
                {
                    object val = GetControlValueByName(container, field.Name, field.FieldType);
                    if (val != null)
                    {
                        field.SetValue(data, Convert.ChangeType(val, field.FieldType));
                    }
                }
            }
        }

        private static void SetControlValueByName(Control container, string baseName, object value, bool isPointer)
        {
            foreach (string prefix in ControlPrefixes)
            {
                var matched = container.Controls.Find(prefix + baseName, true);

                // success
                if (matched.Length > 0)
                {
                    switch (matched[0])
                    {
                        case NumericUpDown nud:
                            decimal dec = Convert.ToDecimal(value ?? nud.Minimum);
                            nud.Value = Math.Max(nud.Minimum, Math.Min(nud.Maximum, dec));
                            break;

                        case TextBox txt:
                            if (isPointer && value is int intVal)
                            {
                                txt.Text = intVal == unchecked((int)uint.MaxValue) 
                                    ? "null" 
                                    : intVal.ToString("X8");
                            }
                            else
                            {
                                txt.Text = value?.ToString() ?? string.Empty;
                            }
                            break;

                        case ComboBox cmb:
                            if (!string.IsNullOrEmpty(cmb.ValueMember))
                            {
                                cmb.SelectedValue = IsNumericType(value) 
                                    ? (object)Convert.ToInt32(value) 
                                    : value;
                            }
                            else if (IsNumericType(value))
                            {
                                cmb.SelectedIndex = Convert.ToInt32(value);
                            }
                            else
                            {
                                cmb.SelectedIndex = -1;
                            }
                            break;

                        case CheckBox chk:
                            chk.Checked = Convert.ToBoolean(value ?? false);
                            break;
                    }

                    return;
                }
            }
        }

        private static object GetControlValueByName(Control container, string baseName, Type targetType)
        {
            foreach (string prefix in ControlPrefixes)
            {
                var matched = container.Controls.Find(prefix + baseName, true);

                // success
                if (matched.Length > 0)
                {
                    Control ctrl = matched[0];
                    switch (ctrl)
                    {
                        case NumericUpDown nud:
                            return nud.Value;

                        case TextBox txt:
                            string str = txt.Text.Trim();

                            if (targetType == typeof(int))
                            {
                                if (string.IsNullOrEmpty(str) || str == "null")
                                {
                                    return unchecked((int)uint.MaxValue);
                                }
                                if (ControlHelper.TryParseOffset(str, out int val1))
                                {
                                    return val1;
                                }

                                return null;
                            }

                            // not pointer
                            if (string.IsNullOrEmpty(str)) return null;
                            return Convert.ChangeType(str, targetType);

                        case ComboBox cmb:
                            object val2 = !string.IsNullOrEmpty(cmb.ValueMember) 
                                ? cmb.SelectedValue 
                                : cmb.SelectedIndex;

                            if (val2 is int intVal && intVal < 0)
                            {
                                return null; // fail
                            }

                            if (val2 != null && targetType != typeof(object))
                            {
                                return Convert.ChangeType(val2, targetType);
                            }

                            return val2;

                        case CheckBox chk:
                            return chk.Checked;

                        default:
                            return null;
                    }
                }
            }

            return null;
        }

        private static bool IsNumericType(object obj)
        {
            if (obj == null) return false;
            switch (Type.GetTypeCode(obj.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.Int16:
                case TypeCode.Int32:
                    return true;
                default:
                    return false;
            }
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
