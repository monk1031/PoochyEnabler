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
        private const string HexPrefix = "0x";

        // store all configs
        private readonly Dictionary<string, HashSet<string>> _configBlocks =  new Dictionary<string, HashSet<string>>();
        // analyzed lines
        private readonly Dictionary<string, int> _iniCacheInt =  new Dictionary<string, int>();
        private readonly Dictionary<string, bool> _iniCacheBool = new Dictionary<string, bool>();

        // get int
        public bool TryReadValue(string key, out int value, int defaultValue = 0)
        {
            if (_iniCacheInt.TryGetValue(key, out value))
            {
                return true;
            }

            value = defaultValue;
            return false;
        }

        // get bool
        public bool TryReadValue(string key, out bool value, bool defaultValue = false)
        {
            if (_iniCacheBool.TryGetValue(key, out value))
            {
                return true;
            }

            value = defaultValue;
            return false;
        }

        // for cmb items, still contain empty lines
        public IniFileReader(string folderPath, ComboBox targetCmb)
        {
            if (!Directory.Exists(folderPath)) return;

            // clear
            _configBlocks.Clear();
            _iniCacheInt.Clear();
            _iniCacheBool.Clear();
            targetCmb.Items.Clear();

            // search for .ini
            foreach (string filePath in Directory.EnumerateFiles(folderPath, "*.ini"))
            {
                // set config name
                string configName = Path.GetFileNameWithoutExtension(filePath);

                // scan lines
                HashSet<string> blockLines = new HashSet<string>(File.ReadLines(filePath, Encoding.UTF8));

                // add to dict and cmb
                _configBlocks[configName] = blockLines;
                targetCmb.Items.Add(configName);
            }

            // select first index
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
                // check empty
                if (!TryParseLine(line, out string key, out string rawString)) continue;

                // check bool
                if (bool.TryParse(rawString, out bool boolValue))
                {
                    _iniCacheBool[key] = boolValue;
                    continue;
                }

                // check pointer
                if (rawString.StartsWith("*"))
                {
                    if (TryReadPointerAddress(rawString.Substring(1), data, out int ptrValue))
                    {
                        _iniCacheInt[key] = ptrValue;
                    }
                    continue;
                }

                // number (hex, dec)
                if (TryParseNumber(rawString, out int numValue))
                {
                    _iniCacheInt[key] = numValue;
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
            if (parts.Length != 2) return false; // fail

            key = parts[0].Trim();
            rawString = parts[1].Trim();
            return true;
        }

        // remove 0x
        private bool TryParseNumber(string rawString, out int parsedValue)
        {
            if (rawString.StartsWith(HexPrefix)) // 0x
            {
                string hexPart = rawString.Substring(HexPrefix.Length);
                return int.TryParse(hexPart, NumberStyles.HexNumber, null, out parsedValue);
            }

            return int.TryParse(rawString, NumberStyles.HexNumber, null, out parsedValue);
        }

        // remove *
        private bool TryReadPointerAddress(string offsetStr, byte[] data, out int parsedValue)
        {
            parsedValue = unchecked((int)uint.MaxValue); // -1

            if (TryParseNumber(offsetStr, out int ptrOffset))
            {
                if (IOHelper.TryReadGbaPointer(ptrOffset, data, out int resultOffset))
                {
                    parsedValue = resultOffset;
                    return true;
                }
            }

            return false;
        }
    }
}
