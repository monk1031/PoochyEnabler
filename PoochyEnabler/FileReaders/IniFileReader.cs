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

        // to get value
        public T ReadValue<T>(string key, T defaultValue = default) where T : struct
        {
            if (_iniCache.TryGetValue(key, out var val))
            {
                Type t = typeof(T);

                if (t == typeof(uint)) // offset
                {
                    return (T)(object)(uint)val;
                }
                else if (t == typeof(int)) // number
                {
                    return (T)(object)(int)(uint)val;
                }
                else if (t == typeof(bool))
                {
                    return (T)(object)(bool)val;
                }
            }

            return defaultValue;
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

        // create _inicache, need romdata to get value
        public void LoadConfig(string selectedConfig, byte[] data)
        {
            if (!_configBlocks.ContainsKey(selectedConfig)) return;

            // analyze lines
            foreach (string line in _configBlocks[selectedConfig])
            {
                if (TryParseLine(line, out string key, out string rawValue))
                {
                    if (TryParseValue(rawValue, data, out object parsedValue))
                    {
                        _iniCache[key] = parsedValue;
                    }
                }
            }
        }

        // remove empty lines
        private bool TryParseLine(string line, out string key, out string rawValue)
        {
            key = string.Empty;
            rawValue = string.Empty;
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";")) return false;

            string[] parts = line.Split('=');
            key = parts[0].Trim();
            rawValue = parts[1].Trim();
            return true;
        }

        // offset -> uint, number -> int, bool -> bool
        private bool TryParseValue(string rawValue, byte[] data, out object parsedValue)
        {
            if (bool.TryParse(rawValue, out bool boolValue))
            {
                parsedValue = boolValue;
                return true;
            }

            switch (rawValue[0])
            {
                case '*':
                    return TryReadPointerAddress(
                        rawValue.Substring(1), data, out parsedValue);

                case '$':
                    return TrySearchBinaryAndReadPointer(
                        rawValue.Substring(1), data, out parsedValue);
            }

            return TryParseNumber(rawValue, out parsedValue);
        }

        private bool TryParseNumber(string rawValue, out object parsedValue)
        {
            if (rawValue.StartsWith(HexPrefix)) // 0x
            {
                string hexPart = rawValue.Substring(HexPrefix.Length);
                if (uint.TryParse(hexPart, NumberStyles.HexNumber, null, out uint hexResult))
                {
                    parsedValue = hexResult;
                    return true;
                }
            }
            else if (uint.TryParse(rawValue, out uint decResult))
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
