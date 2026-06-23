using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

using PoochyEnabler.Helpers;

namespace PoochyEnabler.FileReaders
{
    public class IniFileReader
    {
        private const string BeginPrefix = "[BEGIN][";
        private const string EndPrefix = "[END][";
        private const string Suffix = "]";
        private const string HexPrefix = "0x";

        // store all configs
        private readonly Dictionary<string, List<string>> _configBlocks = 
            new Dictionary<string, List<string>>();
        // analyzed lines
        private readonly Dictionary<string, object> _iniCache = 
            new Dictionary<string, object>();

        public bool TryReadValue<T>(string key, out T? value) where T : struct
        {
            value = null;

            if (_iniCache.TryGetValue(key, out var val))
            {
                if (val == null)
                {
                    return true; // null pointer
                }

                if (typeof(T) == typeof(uint))
                {
                    value = (T)(object)(uint)val;
                    return true;
                }

                if (typeof(T) == typeof(int))
                {
                    value = (T)(object)(int)(uint)val;
                    return true;
                }

                if (typeof(T) == typeof(bool))
                {
                    value = (T)(object)(bool)val;
                    return true;
                }
            }

            return false; // no key
        }

        public uint? ReadOffset(string key)
        {
            return TryReadValue<uint>(key, out uint? value)
                ? value
                : null;
        }

        public int ReadNumber(string key, int defaultValue = 0)
        {
            return TryReadValue<int>(key, out int? value)
                ? value ?? defaultValue
                : defaultValue;
        }

        public bool ReadBool(string key, bool defaultValue = false)
        {
            return TryReadValue<bool>(key, out bool? value)
                ? value ?? defaultValue
                : defaultValue;
        }

        // for cmb items, still contain empty lines
        public IniFileReader(string filePath, ComboBox targetCmb)
        {
            if (!File.Exists(filePath)) return;

            _iniCache.Clear();
            targetCmb.Items.Clear();
            string currentConfigName = string.Empty;
            List<string> currentBlockLines = new List<string>();

            foreach (string line in File.ReadLines(filePath, Encoding.UTF8))
            {
                if (line.StartsWith(BeginPrefix) && line.EndsWith(Suffix)) // [BEGIN][XXXX]
                {
                    int start = BeginPrefix.Length;
                    int length = line.Length - Suffix.Length - BeginPrefix.Length;
                    currentConfigName = line.Substring(start, length);
                    currentBlockLines = new List<string>(); // new
                }
                else if (line.StartsWith(EndPrefix) && line.EndsWith(Suffix)) // [END][XXXX]
                {
                    int start = EndPrefix.Length;
                    int length = line.Length - Suffix.Length - EndPrefix.Length;
                    string endConfigName = line.Substring(start, length);
                    if (currentConfigName == endConfigName) // same?
                    {
                        _configBlocks[currentConfigName] = currentBlockLines;
                        targetCmb.Items.Add(currentConfigName);
                    }
                    currentConfigName = string.Empty;
                    currentBlockLines = new List<string>(); // clear
                }
                else if (!string.IsNullOrEmpty(currentConfigName))
                {
                    currentBlockLines.Add(line);
                }
            }

            if (targetCmb.Items.Count > 0)
            {
                targetCmb.SelectedIndex = 0;
            }
        }

        // create _inicache
        public void LoadConfig(string selectedConfig, byte[] data)
        {
            if (!_configBlocks.ContainsKey(selectedConfig)) return;

            // analyze lines
            foreach (string line in _configBlocks[selectedConfig])
            {
                if (TryParseLine(line, out string key, out string rawString))
                {
                    if (TryParseValue(rawString, data, out object parsedValue))
                    {
                        _iniCache[key] = parsedValue;
                    }
                }
            }
        }

        // remove empty lines
        private bool TryParseLine(string line, out string key, out string rawString)
        {
            key = string.Empty;
            rawString = string.Empty;
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";")) return false;

            string[] parts = line.Split('=');
            key = parts[0].Trim();
            rawString = parts[1].Trim();
            return true;
        }

        private bool TryParseValue(string rawString, byte[] data, out object parsedValue)
        {
            if (bool.TryParse(rawString, out bool boolValue))
            {
                parsedValue = boolValue;
                return true;
            }

            switch (rawString[0])
            {
                case '*':
                    return TryReadPointerAddress(
                        rawString.Substring(1), data, out parsedValue);

                case '$':
                    return TrySearchBinaryAndReadPointer(
                        rawString.Substring(1), data, out parsedValue);
            }

            return TryParseNumber(rawString, out parsedValue);
        }

        // offset, number -> uint? or uint
        private bool TryParseNumber(string rawString, out object parsedValue)
        {
            if (rawString.StartsWith(HexPrefix)) // 0x
            {
                string hexPart = rawString.Substring(HexPrefix.Length);
                if (uint.TryParse(hexPart, NumberStyles.HexNumber, null, out uint hexResult))
                {
                    parsedValue = hexResult;
                    return true;
                }
            }
            else if (uint.TryParse(rawString, out uint decResult))
            {
                parsedValue = decResult;
                return true;
            }

            parsedValue = null;
            return false;
        }

        // 00800000:[EF CD AB 08] -> get 00ABCDEF
        private bool TryReadPointerAddress(string offsetStr, byte[] data, out object parsedValue)
        {
            parsedValue = null;

            if (TryParseNumber(offsetStr, out object offsetValue))
            {
                uint ptrOffset = (uint)offsetValue;
                if (IOHelper.TryReadPointer(ptrOffset, data, out uint? resultOffset))
                {
                    parsedValue = resultOffset; // contain null
                    return true;
                }
            }

            return false;
        }

        // search [AB CD EF 12 34 56], and then read that pointer
        private bool TrySearchBinaryAndReadPointer(string searchStr, byte[] data, out object parsedValue)
        {
            parsedValue = null;

            string[] parts = searchStr.Split(',');
            string hexString = parts[0].Trim();
            uint additionalOffset = 0;
            if (parts.Length > 1 && TryParseNumber(parts[1].Trim(), out object parsedOffsetValue))
            {
                additionalOffset = (uint)parsedOffsetValue;
            }

            // get bytes to search for
            byte[] patternBytes = new byte[hexString.Length / Constants.CharPerByte];
            for (int i = 0; i < patternBytes.Length; i++)
            {
                string targetStr = hexString.Substring(i * Constants.CharPerByte, Constants.CharPerByte);
                patternBytes[i] = Convert.ToByte(targetStr, Constants.HexBase);
            }

            // searching
            bool isPatternFound = false;
            uint startOffset = 0;
            for (uint i = 0; i <= data.Length - patternBytes.Length; i++)
            {
                bool isMatch = true;
                for (int j = 0; j < patternBytes.Length; j++)
                {
                    if (data[i + j] != patternBytes[j])
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                {
                    startOffset = i;
                    isPatternFound = true;
                    break;
                }
            }

            if (!isPatternFound)
            {
                return false;
            }

            uint ptrOffset = startOffset + (uint)patternBytes.Length + additionalOffset;
            if (IOHelper.TryReadPointer(ptrOffset, data, out uint? resultOffset))
            {
                parsedValue = resultOffset; // contain null
                return true;
            }

            return false;
        }
    }
}
