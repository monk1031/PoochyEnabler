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
    }
}
