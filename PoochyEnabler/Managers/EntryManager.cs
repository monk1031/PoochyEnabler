using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using PoochyEnabler.FileReaders;
using PoochyEnabler.Helpers;

namespace PoochyEnabler.Managers
{
    public class EntryManager<T> where T : class, new()
    {
        private readonly byte[] _romData;
        private readonly IniFileReader _config;
        private readonly TblFileReader _charmap;
        private Dictionary<string, int> _dynamicLengths;

        public List<T> Entries { get; set; } = new List<T>();
        public int Offset { get; set; } // base offset
        public int Count { get; set; }

        public EntryManager(
            byte[] romData,
            IniFileReader config,
            TblFileReader charmap,
            Dictionary<string, int> dynamicLengths = null)
        {
            _romData = romData;
            _config = config;
            _charmap = charmap;
            _dynamicLengths = dynamicLengths;
        }

        // type A : when need inicache
        public void Load(string offsetKey, string countKey, bool autoBuildLengths = false)
        {
            if (autoBuildLengths)
            {
                _dynamicLengths = new Dictionary<string, int>();
                var fields = typeof(T).GetFields()
                                      .OrderBy(f => f.MetadataToken)
                                      .ToArray();

                foreach (var field in fields)
                {
                    var attr = field.GetCustomAttribute<DynamicStringAttribute>();
                    if (attr == null) continue;

                    // EntryLength
                    if (!string.IsNullOrEmpty(attr.EntryLength) &&
                        !_dynamicLengths.ContainsKey(attr.EntryLength))
                    {
                        if (_config.TryReadValue(attr.EntryLength, out int len))
                        {
                            _dynamicLengths[attr.EntryLength] = len;
                        }
                    }

                    // AllowedLength
                    if (!string.IsNullOrEmpty(attr.AllowedLength) &&
                        !_dynamicLengths.ContainsKey(attr.AllowedLength))
                    {
                        if (_config.TryReadValue(attr.AllowedLength, out int len))
                        {
                            _dynamicLengths[attr.AllowedLength] = len;
                        }
                    }
                }
            }

            if (_config.TryReadValue(offsetKey, out int offsetValue) &&
                _config.TryReadValue(countKey, out int countValue))
            {
                Load(offsetValue, countValue);
            }
        }

        // type B : directly
        public void Load(int offset, int count)
        {
            Offset = offset;
            Count = count;
            Entries = IOHelper.ReadStructures<T>(
                _romData, 
                Offset, 
                Count, 
                _charmap, 
                _dynamicLengths);
        }

        // write entries
        public void Save(
            int index,
            int count = 1,
            bool appendTerminator = true,
            byte paddingByte1 = Constants.FreeSpaceByte,
            byte paddingByte2 = Constants.PaddingByte)
        {
            IOHelper.WriteStructures(
                _romData,
                Offset,
                index,
                Entries.Skip(index).Take(count),
                _charmap,
                _dynamicLengths,
                appendTerminator,
                paddingByte1,
                paddingByte2);
        }
    }

    /* ---------------------------------------------------------------- */

    public class ClassNameEntry
    {
        [DynamicString("ClassNameEntryLength")]
        public string _ClassName;
    }

    public class ClassPrizeMultiplierEntry
    {
        public byte _ClassIdx;
        public byte _ClassPrizeMulti;
        public byte _Padding1;
        public byte _Padding2;
    }

    public class ClassEncounterMusicEntry
    {
        public ushort ClassEncounterMusic;
    }

    public class ClassBattleMusicEntry
    {
        public ushort ClassBattleMusic;
    }

    public class ClassPokeBallEntry
    {
        public byte ClassPokeBall;
    }

    public class ClassBaseIVEntry
    {
        public byte ClassBaseIv;
    }

    public class TrainerImageEntry
    {
        public uint pImageOffset;
        public ushort _DecompressedSize;
        public byte _SpriteIndex;
        public byte _Padding1;
    }

    public class TrainerPaletteEntry
    {
        public uint pPaletteOffset;
        public byte _SpriteIndex;
        public byte _Padding1;
        public byte _Padding2;
        public byte _Padding3;
    }

    public class TrainerYOffsetEntry
    {
        public byte _TileCount;
        public byte YPosition;
        public byte _Padding1;
        public byte _Padding2;
    }

    public class TrainerAnimPointerEntry
    {
        public uint pAnimPointer;
    }
}
