using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using PoochyEnabler.FileReaders;

namespace PoochyEnabler.Helpers
{
    public static class IOHelper
    {
        public static uint ReadUShortLE(byte[] data, uint offset)
        {
            return (uint)(data[offset]
                 | (data[offset + 1] << Constants.BitsPerByte));
        }

        public static uint ReadUIntLE(byte[] data, uint offset)
        {
            return (uint)data[offset]
                 | ((uint)data[offset + 1] << Constants.BitsPerByte)
                 | ((uint)data[offset + 2] << (Constants.BitsPerByte * 2))
                 | ((uint)data[offset + 3] << (Constants.BitsPerByte * 3));
        }

        public static bool TryReadPointer(uint ptrOffset, byte[] data, out uint? resultOffset)
        {
            uint rawAddr = ReadUIntLE(data, ptrOffset);

            // check null pointer
            if (rawAddr == 0)
            {
                resultOffset = null;
                return true;
            }

            // valid?
            if (rawAddr < Constants.BaseAddr)
            {
                resultOffset = null;
                return false;
            }

            resultOffset = rawAddr - Constants.BaseAddr;
            return true;
        }

        // align for variable data
        public static void WriteDataToRom(
            byte[] romData,
            uint offset,
            byte[] bytes,
            bool align = false,
            byte alignPaddingByte = Constants.PaddingByte)
        {
            Array.Copy(bytes, 0, romData, offset, bytes.Length);

            if (align)
            {
                uint endOffset = offset + (uint)bytes.Length;
                uint remainder = endOffset % sizeof(uint);

                if (remainder != 0)
                {
                    uint paddingCount = sizeof(uint) - remainder;
                    for (uint i = 0; i < paddingCount; i++)
                    {
                        uint padOffset = endOffset + i;
                        romData[padOffset] = alignPaddingByte;
                    }
                }
            }
        }

        // read and create, string -> variable
        public static List<T> ReadStructures<T>(
            byte[] data,
            uint offset,
            int count, 
            TblFileReader tblReader,
            Dictionary<string, int> dynamicLengths = null) where T : new()
        {
            var list = new List<T>(count);
            int currentOffset = (int)offset;
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                IntPtr basePtr = handle.AddrOfPinnedObject();

                FieldInfo[] fields = typeof(T)
                    .GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .OrderBy(f => f.MetadataToken)
                    .ToArray();

                for (int i = 0; i < count; i++)
                {
                    var item = new T();

                    foreach (var field in fields)
                    {
                        if (field.FieldType == typeof(string))
                        {
                            var attr = field.GetCustomAttribute<DynamicStringAttribute>();
                            if (TryGetLength(attr.EntryLength, dynamicLengths, out int length) && length > 0)
                            {
                                string strVal = tblReader.BytesToString(data, currentOffset, length);
                                field.SetValue(item, strVal);
                                currentOffset += length;
                            }
                        }
                        else if (field.FieldType.IsValueType)
                        {
                            int typeSize = Marshal.SizeOf(field.FieldType);
                            object val = Marshal.PtrToStructure(basePtr + currentOffset, field.FieldType);
                            field.SetValue(item, val);
                            currentOffset += typeSize;
                        }
                    }

                    list.Add(item);
                }
            }
            finally
            {
                handle.Free();
            }

            return list;
        }

        // write  structures
        // paddingByte1: Pad up to max characters
        // paddingByte2: Pad up to data length
        public static void WriteStructures<T>(
            byte[] data,
            uint offset,
            IEnumerable<T> items,
            TblFileReader tblReader,
            Dictionary<string, int> dynamicLengths = null,
            bool appendTerminator = true,
            byte paddingByte1 = Constants.FreeSpaceByte,
            byte paddingByte2 = Constants.PaddingByte)
        {
            int currentOffset = (int)offset;
            var handle = GCHandle.Alloc(data, GCHandleType.Pinned);

            try
            {
                IntPtr basePtr = handle.AddrOfPinnedObject();

                FieldInfo[] fields = typeof(T)
                    .GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .OrderBy(f => f.MetadataToken)
                    .ToArray();

                foreach (var item in items)
                {
                    foreach (var field in fields)
                    {
                        if (field.FieldType == typeof(string))
                        {
                            var attr = field.GetCustomAttribute<DynamicStringAttribute>();
                            if (!TryGetLength(attr.EntryLength, dynamicLengths, out int entryLength) || entryLength <= 0) continue;
                            string strVal = (string)field.GetValue(item) ?? string.Empty;

                            if (TryGetLength(attr.AllowedLength, dynamicLengths, out int allowedLength) && allowedLength > 0)
                            {
                                byte[] rawBytes = tblReader.StringToBytes(strVal, false);
                                var finalBytes = new List<byte>(rawBytes);

                                if (appendTerminator)
                                {
                                    finalBytes.Add(Constants.FreeSpaceByte);

                                    while (finalBytes.Count < allowedLength)
                                    {
                                        finalBytes.Add(paddingByte1);
                                    }
                                }

                                while (finalBytes.Count < entryLength)
                                {
                                    finalBytes.Add(paddingByte2);
                                }

                                Array.Copy(finalBytes.ToArray(), 0, data, currentOffset, entryLength);
                            }
                            else
                            {
                                byte[] result = tblReader.StringToBytes(strVal, appendTerminator, entryLength, paddingByte2);
                                Array.Copy(result, 0, data, currentOffset, entryLength);
                            }

                            currentOffset += entryLength;
                        }
                        else if (field.FieldType.IsValueType)
                        {
                            int typeSize = Marshal.SizeOf(field.FieldType);
                            object value = field.GetValue(item);
                            if (value != null)
                            {
                                Marshal.StructureToPtr(value, basePtr + currentOffset, false);
                            }

                            currentOffset += typeSize;
                        }
                    }
                }
            }
            finally
            {
                handle.Free();
            }
        }

        private static bool TryGetLength(string key, Dictionary<string, int> dynamicLengths, out int length)
        {
            length = 0;
            return dynamicLengths != null && dynamicLengths.TryGetValue(key, out length);
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class DynamicStringAttribute : Attribute
    {
        public string EntryLength { get; }
        public string AllowedLength { get; }

        public DynamicStringAttribute(string entryLength, string allowedLength = null)
        {
            EntryLength = entryLength;
            AllowedLength = allowedLength;
        }
    }
}
