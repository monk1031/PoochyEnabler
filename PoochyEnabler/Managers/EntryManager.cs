using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using PoochyEnabler.FileReaders;
using PoochyEnabler.Helpers;

namespace PoochyEnabler.Managers
{
    public class EntryManager<T> where T : class, new()
    {
        private readonly byte[] _romData;
        private readonly TblFileReader _charmap;
        private readonly Dictionary<string, int> _dynamicLengths;

        public List<T> Entry { get; set; } = new List<T>();
        public uint Offset { get; set; }
        public int Count { get; set; }

        public EntryManager(
            byte[] romData,
            TblFileReader charmap,
            Dictionary<string, int> dynamicLengths = null)
        {
            _romData = romData;
            _charmap = charmap;
            _dynamicLengths = dynamicLengths;
        }

        // load all entries
        public void Load(uint offset, int count)
        {
            Offset = offset;
            Count = count;
            Entry = IOHelper.ReadStructures<T>(
                _romData, 
                offset, 
                count, 
                _charmap, 
                _dynamicLengths);
        }

        // save one entry
        // paddingByte1: Pad up to max characters
        // paddingByte2: Pad up to data length
        public void Save(
            int idx,
            bool appendTerminator = true,
            byte paddingByte1 = Constants.FreeSpaceByte,
            byte paddingByte2 = Constants.PaddingByte)
        {
            // calc offset
            int entrySize = GetEntrySize();
            uint offset = Offset + (uint)(idx * entrySize);

            // one entry
            IOHelper.WriteStructures(
                _romData,
                offset,
                new List<T> { Entry[idx] },
                _charmap,
                _dynamicLengths,
                appendTerminator,
                paddingByte1,
                paddingByte2);
        }

        // calc enrty size
        public int GetEntrySize()
        {
            int size = 0;

            foreach (var field in GetOrderedFields())
            {
                if (field.FieldType == typeof(string))
                {
                    if (field.GetCustomAttribute<DynamicStringAttribute>() is DynamicStringAttribute attr &&
                        _dynamicLengths != null &&
                        _dynamicLengths.TryGetValue(attr.EntryLength, out int len))
                    {
                        size += len;
                    }
                }
                else if (field.FieldType.IsValueType)
                {
                    size += Marshal.SizeOf(field.FieldType);
                }
            }

            return size;

            // helper
            FieldInfo[] GetOrderedFields()
            {
                return typeof(T)
                    .GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .OrderBy(f => f.MetadataToken)
                    .ToArray();
            }
        }
    }
}
