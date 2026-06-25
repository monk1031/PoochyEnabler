using System;
using System.Collections.Generic;
using System.Linq;

using PoochyEnabler.FileReaders;
using PoochyEnabler.Helpers;

namespace PoochyEnabler.Managers
{
    public class EntryManager<T> where T : class, new()
    {
        private readonly byte[] _romData;
        private readonly IniFileReader _config;
        private readonly TblFileReader _charmap;
        private readonly Dictionary<string, int> _dynamicLengths;

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
        public void Load(string offsetKey, string countKey)
        {
            if (_config.TryReadValue(offsetKey, out int offsetValue) &&
                _config.TryReadValue(countKey, out int countValue))
            {
                // type B
                Load(offsetValue, countValue);
            }
        }

        // type B : directly
        public void Load(int offset, int count)
        {
            Offset = offset;
            Count = count;
            Entries = IOHelper.ReadStructures<T>(_romData, Offset, Count, _charmap, _dynamicLengths);
        }
    }

    /* ---------------------------------------------------------------- */

    public class TrainerClassNameEntry
    {
        [DynamicString("TrainerClassNameEntryLength")]
        public string _ClassName;
    }

    public class TrainerClassPrizeMultiplierEntry
    {
        public byte _ClassIdx;
        public byte _ClassPrizeMulti;
        public byte _Padding1;
        public byte _Padding2;
    }

    public class TrainerClassEncounterMusicEntry
    {
        public ushort ClassEncounterMusic;
    }

    public class TrainerClassBattleMusicEntry
    {
        public ushort ClassBattleMusic;
    }

    public class TrainerClassPokeBallEntry
    {
        public byte ClassPokeBall;
    }

    public class TrainerClassBaseIVEntry
    {
        public byte ClassBaseIv;
    }
}
