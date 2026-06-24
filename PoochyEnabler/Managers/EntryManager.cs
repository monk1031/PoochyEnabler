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

        public List<T> Entry { get; set; } = new List<T>();
        public uint Offset { get; set; } // base offset
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

        // when need inicache
        public void Load(string offsetKey, string countKey)
        {
            // calc offse, count -> create table
            if (_config.TryReadValue<uint>(offsetKey, out var offsetValue) && offsetValue != null &&
                _config.TryReadValue<int>(countKey, out var countValue) && countValue != null)
            {
                Offset = offsetValue.Value;
                Count = countValue.Value;
                Entry = IOHelper.ReadStructures<T>(_romData, Offset, Count, _charmap, _dynamicLengths);
            }
        }
    }
}
