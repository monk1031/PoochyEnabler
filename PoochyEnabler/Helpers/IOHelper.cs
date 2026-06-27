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
        // because want to cast to int 
        public static uint ReadUShortLE(byte[] data, int offset)
        {
            return (uint)(data[offset]
                 | (data[offset + 1] << Constants.BitsPerByte));
        }

        public static uint ReadUIntLE(byte[] data, int offset)
        {
            return (uint)data[offset]
                 | ((uint)data[offset + 1] << Constants.BitsPerByte)
                 | ((uint)data[offset + 2] << (Constants.BitsPerByte * 2))
                 | ((uint)data[offset + 3] << (Constants.BitsPerByte * 3));
        }

        // uint -> address (consider bese address)
        // int -> offset
        public static bool TryReadGbaPointer(int ptrOffset, byte[] data, out int resultOffset)
        {
            uint rawAddr = ReadUIntLE(data, ptrOffset);

            // check null pointer
            if (rawAddr == 0)
            {
                resultOffset = -1;
                return true;
            }

            // valid?
            if (rawAddr < Constants.BaseAddr)
            {
                resultOffset = -1;
                return false;
            }

            resultOffset = (int)(rawAddr - Constants.BaseAddr);
            return true;
        }

        // align for variable length
        public static void WriteDataToRom(
            byte[] romData,
            int offset,
            byte[] bytes,
            bool align = true,
            byte alignPaddingByte = Constants.PaddingByte)
        {
            Array.Copy(bytes, 0, romData, offset, bytes.Length);

            if (align)
            {
                int endOffset = offset + bytes.Length;
                int remainder = endOffset % sizeof(uint);

                if (remainder != 0)
                {
                    int paddingCount = sizeof(uint) - remainder;
                    for (int i = 0; i < paddingCount; i++)
                    {
                        int padOffset = endOffset + i;
                        romData[padOffset] = alignPaddingByte;
                    }
                }
            }
        }

        // string -> variable length
        public static List<T> ReadStructures<T>(
            byte[] data,
            int baseOffset,
            int count, 
            TblFileReader tblReader,
            Dictionary<string, int> dynamicLengths = null) where T : new()
        {
            var list = new List<T>(count);
            int currentOffset = baseOffset;
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
                                string str = tblReader.BytesToString(data, currentOffset, length);
                                field.SetValue(item, str);
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

        // paddingByte1: Pad up to allowed length
        // paddingByte2: Pad up to entry length
        public static void WriteStructures<T>(
           byte[] data,
           int baseOffset,
           int startIndex,        // target index
           IEnumerable<T> items,  // structures to write
           TblFileReader tblReader,
           Dictionary<string, int> dynamicLengths = null,
           bool appendTerminator = true,
           byte paddingByte1 = Constants.FreeSpaceByte,
           byte paddingByte2 = Constants.PaddingByte)
        {
            // calc structure size
            int recordSize = GetStructureSize<T>(dynamicLengths);
            // calc target offset
            int currentOffset = baseOffset + startIndex * recordSize;

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
                            string str = (string)field.GetValue(item) ?? string.Empty;

                            if (TryGetLength(attr?.AllowedLength, dynamicLengths, out int allowedLength) && allowedLength > 0)
                            {
                                byte[] rawBytes = tblReader.StringToBytes(str, false); // without 0xFF
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
                                byte[] result = tblReader.StringToBytes(str, appendTerminator, entryLength, paddingByte2);
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

        private static bool TryGetLength(
            string key, 
            Dictionary<string, int> dynamicLengths,
            out int length)
        {
            length = 0;
            return key != null && // case : allowed length = null
                dynamicLengths != null && 
                dynamicLengths.TryGetValue(key, out length);
        }

        public static int GetStructureSize<T>(Dictionary<string, int> dynamicLengths = null)
        {
            int size = 0;
            FieldInfo[] fields = typeof(T)
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .OrderBy(f => f.MetadataToken)
                .ToArray();

            foreach (var field in fields)
            {
                if (field.FieldType == typeof(string))
                {
                    var attr = field.GetCustomAttribute<DynamicStringAttribute>();
                    if (TryGetLength(attr.EntryLength, dynamicLengths, out int length) && length > 0)
                    {
                        size += length;
                    }
                }
                else if (field.FieldType.IsValueType)
                {
                    size += Marshal.SizeOf(field.FieldType);
                }
            }

            return size;
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
