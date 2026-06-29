using System;
using System.Collections.Generic;
using System.Linq;

namespace PoochyEnabler.Managers
{
    public class EntryCountManager
    {






        public CompiledPattern ParsePattern(string patternStr)
        {
            var compiledPattern = new CompiledPattern();
            string[] parts = patternStr.Split(' ');

            for (int i = 0; i < parts.Length; i++)
            {
                var token = new PatternToken();

                // check type
                TokenType type = DetermineKind(parts[i]);
                token.Type = type;

                // check value
                token.Value =
                    type == TokenType.Constant
                        ? Convert.ToByte(parts[i], Constants.HexBase)
                        : (byte)0;

                // check be,le
                if (parts[i].Contains("<"))
                {
                    int start = parts[i].IndexOf('<');
                    int end = parts[i].IndexOf('>');
                    string endian = parts[i].Substring(start + 1, end - start - 1);
                    token.IsLittleEndian = endian == "LE";
                }
                else
                {
                    token.IsLittleEndian = false; // default
                }

                // check range
                if (parts[i].Contains('['))
                {
                    int start = parts[i].IndexOf('[');
                    int end = parts[i].IndexOf(']');
                    string range = parts[i].Substring(start + 1, end - start - 1);
                    string[] values = range.Split('-');

                    token.Min = Convert.ToUInt32(values[0], Constants.HexBase);
                    token.Max = Convert.ToUInt32(values[1], Constants.HexBase);
                }
                else
                {
                    token.Min = 0;
                    token.Max = uint.MaxValue;
                }

                compiledPattern.Tokens.Add(token);
            }

            return compiledPattern;

            // helper
            TokenType DetermineKind(string part)
            {
                if (part == "??")
                {
                    return TokenType.Wildcard;
                }

                if (part == "ptr")
                {
                    return TokenType.Pointer;
                }

                if (part.Length == 2)
                {
                    return TokenType.Constant;
                }

                switch (part[0])
                {
                    case 'b':
                        return TokenType.Byte;

                    case 's':
                        return TokenType.UInt16;

                    case 'u':
                        return TokenType.UInt32;
                }

                return TokenType.None;
            }
        }

        public class CompiledPattern
        {
            public List<PatternToken> Tokens { get; set; }
            public int Length 
            {
                get => Tokens.Sum(token => token.Length);
            }

            public CompiledPattern()
            {
                Tokens = new List<PatternToken>();
            }
        }

        public class PatternToken
        {
            public TokenType Type { get; set; }
            public byte Value { get; set; }
            public bool IsLittleEndian { get; set; }
            public uint Min { get; set; }
            public uint Max { get; set; }
            public int Length
            {
                get
                {
                    switch (Type)
                    {
                        case TokenType.Constant:
                        case TokenType.Wildcard:
                        case TokenType.Byte:
                            return 1;
                        case TokenType.UInt16:
                            return 2;
                        case TokenType.UInt32:
                        case TokenType.Pointer:
                            return 4;
                        default:
                            return 0;
                    }
                }
            }

        }

        public enum TokenType
        {
            Constant,   // e.g. 0C
            Wildcard,   // ??
            Pointer,    // ptr
            Byte,       // e.g. b[0x00-0x03]
            UInt16,
            UInt32,     // e.g. u<LE>[0x00000000-0x10000000]
            None
        }
    }
}
