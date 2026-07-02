using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PoochyEnabler.FileReaders
{
    public class TblFileReader
    {
        // store all lang lines
        private readonly Dictionary<string, HashSet<string>> _langBlocks =
            new Dictionary<string, HashSet<string>>();

        // to search for
        private readonly ByteTrieNode _byteTrieRoot = new ByteTrieNode();
        private readonly StringTrieNode _stringTrieRoot = new StringTrieNode();

        /* ---------------------------------------------------------------- */

        // create dict and cmb
        public TblFileReader(string folderPath, ComboBox targetCmb)
        {
            if (!Directory.Exists(folderPath)) return;

            // clear
            _langBlocks.Clear();
            _byteTrieRoot.Clear();
            _stringTrieRoot.Clear();
            targetCmb.Items.Clear();

            // search for .tbl
            foreach (string filePath in Directory.EnumerateFiles(folderPath, "*.tbl"))
            {
                // get config name
                string langName = Path.GetFileNameWithoutExtension(filePath);

                // scan lines
                HashSet<string> blockLines =
                    new HashSet<string>(File.ReadLines(filePath, Encoding.UTF8));

                // add to dict and cmb
                _langBlocks[langName] = blockLines;
                targetCmb.Items.Add(langName);
            }

            // select first index
            if (targetCmb.Items.Count > 0)
            {
                targetCmb.SelectedIndex = 0;
            }

        }

        public void LoadLang(string selectedLang)
        {
            if (!_langBlocks.ContainsKey(selectedLang)) return;

            foreach (string line in _langBlocks[selectedLang])
            {
                // comment
                if (string.IsNullOrEmpty(line) || line.StartsWith(";")) continue;

                string[] parts = line.Split('=');
                string hexKey = parts[0].Replace(" ", ""); // assume 2 bytes or more, e.g. [AB CD]
                string value = parts[1]; // char

                // key -> bytes
                int byteLen = hexKey.Length / Constants.CharPerByte;
                byte[] bytes = new byte[byteLen];
                for (int i = 0; i < byteLen; i++)
                {
                    string targetStr = hexKey.Substring(i * Constants.CharPerByte, Constants.CharPerByte);
                    bytes[i] = Convert.ToByte(targetStr, Constants.HexBase);
                }

                // byte to string
                ByteTrieNode currentByteNode = _byteTrieRoot;
                foreach (byte b in bytes)
                {
                    if (!currentByteNode.Children.TryGetValue(b, out ByteTrieNode next))
                    {
                        next = new ByteTrieNode();
                        currentByteNode.Children[b] = next;
                    }
                    currentByteNode = next;
                }
                currentByteNode.Value = value;
                currentByteNode.IsTerminal = true;

                // string to byte
                if (!string.IsNullOrEmpty(value))
                {
                    StringTrieNode currentStrNode = _stringTrieRoot;
                    foreach (char c in value)
                    {
                        if (!currentStrNode.Children.TryGetValue(c, out StringTrieNode next))
                        {
                            next = new StringTrieNode();
                            currentStrNode.Children[c] = next;
                        }
                        currentStrNode = next;
                    }
                    currentStrNode.Value = bytes;
                    currentStrNode.IsTerminal = true;
                }
            }
        }

        /* ---------------------------------------------------------------- */

        public string BytesToString(byte[] bytes, int offset = 0, int? maxLength = null)
        {
            if (bytes == null) return string.Empty;
            StringBuilder result = new StringBuilder();

            // determine scope
            int calcLength = bytes.Length - offset;
            int length = 
                maxLength.HasValue
                ? Math.Min(calcLength, maxLength.Value)
                : calcLength;

            int i = 0;
            while (i < length)
            {
                int currentIdx = offset + i;
                byte currentByte = bytes[currentIdx];

                // terminator
                if (currentByte == Constants.StrTerminatorByte)
                {
                    break;
                }

                // newline
                if (currentByte == Constants.StrNewlineByte)
                {
                    result.Append(Environment.NewLine);
                    i++;
                    continue;
                }

                // search
                int matchLength = 0;
                string matchedString = null;
                ByteTrieNode currentNode = _byteTrieRoot;

                for (int j = 0; j < length - i; j++)
                {
                    byte b = bytes[currentIdx + j];
                    if (currentNode.Children.TryGetValue(b, out ByteTrieNode next))
                    {
                        currentNode = next;
                        if (currentNode.IsTerminal)
                        {
                            matchLength = j + 1;
                            matchedString = currentNode.Value;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (matchLength > 0 && matchedString != null)
                {
                    result.Append(matchedString);
                    i += matchLength;
                }
                else
                {
                    // not exist
                    i++;
                }
            }

            return result.ToString();
        }

        public byte[] StringToBytes(
            string text,
            bool appendTerminator = true,
            int targetLength = -1,
            byte paddingByte = Constants.PaddingByte)
        {
            text = text ?? string.Empty;
            List<byte> result = new List<byte>();

            int i = 0;
            while (i < text.Length)
            {
                // newline
                if (text[i] == '\r' && text[i + 1] == '\n')
                {
                    result.Add(Constants.StrNewlineByte);
                    i += 2;
                    continue;
                }

                int matchLength = 0;
                byte[] matchedBytes = null;
                StringTrieNode currentNode = _stringTrieRoot;

                for (int j = i; j < text.Length; j++)
                {
                    char c = text[j];
                    if (currentNode.Children.TryGetValue(c, out StringTrieNode next))
                    {
                        currentNode = next;
                        if (currentNode.IsTerminal)
                        {
                            matchLength = j - i + 1;
                            matchedBytes = currentNode.Value;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (matchLength > 0 && matchedBytes != null)
                {
                    result.AddRange(matchedBytes);
                    i += matchLength;
                }
                else
                {
                    // not exist
                    i++;
                }
            }

            // terminator?
            if (appendTerminator)
            {
                result.Add(Constants.StrTerminatorByte);
            }

            // padding?
            if (targetLength > 0)
            {
                while (result.Count < targetLength)
                {
                    result.Add(paddingByte);
                }
            }

            return result.ToArray();
        }

        private class ByteTrieNode
        {
            public Dictionary<byte, ByteTrieNode> Children { get; } =
                new Dictionary<byte, ByteTrieNode>();
            public string Value { get; set; }
            public bool IsTerminal { get; set; }

            public void Clear()
            {
                Children.Clear();
                Value = null;
                IsTerminal = false;
            }
        }

        private class StringTrieNode
        {
            public Dictionary<char, StringTrieNode> Children { get; } = 
                new Dictionary<char, StringTrieNode>();
            public byte[] Value { get; set; }
            public bool IsTerminal { get; set; }

            public void Clear()
            {
                Children.Clear();
                Value = null;
                IsTerminal = false;
            }
        }
    }
}
