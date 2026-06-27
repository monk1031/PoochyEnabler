using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PoochyEnabler.Managers
{
    public class ImageManager
    {
        private const int LZ77HeaderSize = 0x4;
        private const int LZ77HeaderIdentifier = 0x10;
        private const int LZ77MaxDistance = 4096;
        private const int LZ77MaxLength = 18;
        private const int LZ77MinMatchLength = 3;
        private const int LZ77MinSafeDistance = 2;
        private const int LZ77CompressedUnitSize = 2;

        // LZ77 decompress
        public byte[] DecompressLZ77(byte[] romData, int baseOffset)
        {
            // read header
            int header = BitConverter.ToInt32(romData, baseOffset);
            int decompressedSize = header >> Constants.BitsPerByte;
            var result = new byte[decompressedSize];

            int srcPos = baseOffset + LZ77HeaderSize;
            int dstPos = 0;

            // process all blocks
            while (dstPos < decompressedSize)
            {
                // read flag byte
                byte flagByte = romData[srcPos++];

                // process 8 units
                for (int i = 0; i < Constants.BitsPerByte; i++)
                {
                    if (dstPos >= decompressedSize) break;

                    bool isCompressed = (flagByte & (1 << (Constants.BitsPerByte - 1 - i))) != 0;

                    if (isCompressed)
                    {
                        // read compressed info
                        byte byte0 = romData[srcPos];
                        byte byte1 = romData[srcPos + 1];
                        srcPos += LZ77CompressedUnitSize;

                        int length = (byte0 >> Constants.NibbleShift) + LZ77MinMatchLength;
                        int offset = (((byte0 & Constants.NibbleMask) << Constants.BitsPerByte) | byte1) + 1;
                        int copySrc = dstPos - offset;

                        // copy previous data
                        for (int j = 0; j < length; j++)
                        {
                            if (dstPos >= decompressedSize) break;
                            result[dstPos++] = result[copySrc++];
                        }
                    }
                    else
                    {
                        // copy raw byte
                        result[dstPos++] = romData[srcPos++];
                    }
                }
            }

            return result;
        }

        // LZ77 compress
        public byte[] CompressLZ77(byte[] imageData)
        {
            int length = imageData.Length;
            var result = new List<byte>(length / Constants.PixelsPerByte4Bpp);

            // write header
            result.Add((byte)LZ77HeaderIdentifier);
            for (int i = 0; i < LZ77HeaderSize - 1; i++)
            {
                result.Add((byte)((length >> (i * Constants.BitsPerByte)) & Constants.ByteMask));
            }

            int pos = 0;

            // process all data
            while (pos < length)
            {
                int flagPos = result.Count;
                byte flag = 0;
                result.Add(0);

                // process 8 units
                for (int i = 0; i < Constants.BitsPerByte; i++)
                {
                    if (pos >= length) break;

                    // search longest match
                    var (bestDistance, bestLength) = FindLongestMatch(imageData, pos);

                    if (bestLength >= LZ77MinMatchLength)
                    {
                        // write compressed data
                        flag |= (byte)(1 << (Constants.BitsPerByte - 1 - i));

                        int offsetVal = bestDistance - 1;
                        int lenVal = bestLength - LZ77MinMatchLength;

                        result.Add((byte)(
                            ((lenVal & Constants.NibbleMask) << Constants.NibbleShift) |
                            ((offsetVal >> Constants.BitsPerByte) & Constants.NibbleMask)));
                        result.Add((byte)(offsetVal & Constants.ByteMask));

                        pos += bestLength;
                    }
                    else
                    {
                        // write raw byte
                        result.Add(imageData[pos++]);
                    }
                }

                // update flag byte
                result[flagPos] = flag;
            }

            // align to 4 bytes
            while (result.Count % sizeof(uint) != 0)
            {
                result.Add(0);
            }

            return result.ToArray();
        }

        // find longest match
        private (int distance, int length) FindLongestMatch(byte[] data, int pos)
        {
            int maxDist = Math.Min(pos, LZ77MaxDistance);
            int maxLen = Math.Min(data.Length - pos, LZ77MaxLength);

            // no valid match
            if (maxDist < LZ77MinSafeDistance || maxLen < LZ77MinMatchLength)
            {
                return (0, 0);
            }

            int bestLength = 0;
            int bestDistance = 0;

            // search all distances
            for (int dist = LZ77MinSafeDistance; dist <= maxDist; dist++)
            {
                int len = 0;
                while (len < maxLen && data[pos - dist + len] == data[pos + len])
                {
                    len++;
                }

                // keep best match
                if (len > bestLength)
                {
                    bestLength = len;
                    bestDistance = dist;

                    if (bestLength == LZ77MaxLength) break;
                }
            }

            return (bestDistance, bestLength);
        }

        // load palette
        public Color[] DecompressPalette(byte[] romData, int offset, bool isCompressed)
        {
            byte[] paletteData;

            // get palette data
            if (isCompressed)
            {
                paletteData = DecompressLZ77(romData, offset);
            }
            else
            {
                paletteData = new byte[Constants.PalColorCount * Constants.BytesPerColor];
                Array.Copy(romData, offset, paletteData, 0, paletteData.Length);
            }

            var colors = new Color[Constants.PalColorCount];

            // convert GBA colors
            for (int i = 0; i < Constants.PalColorCount; i++)
            {
                int byteIndex = i * Constants.BytesPerColor;
                if (byteIndex + 1 >= paletteData.Length) break;

                int temp = (paletteData[byteIndex + 1] << Constants.BitsPerByte) | paletteData[byteIndex];

                int r = ((temp & Constants.RedMask) >> Constants.RedShift) * Constants.ColorChannelMulti;
                int g = ((temp & Constants.GreenMask) >> Constants.GreenShift) * Constants.ColorChannelMulti;
                int b = ((temp & Constants.BlueMask) >> Constants.BlueShift) * Constants.ColorChannelMulti;

                colors[i] = Color.FromArgb(255, r, g, b);
            }

            return colors;
        }

        // save palette
        public byte[] CompressPalette(Color[] colors, bool isCompressed)
        {
            var paletteData = new byte[Constants.PalColorCount * Constants.BytesPerColor];
            int count = Math.Min(Constants.PalColorCount, colors.Length);

            // convert to GBA colors
            for (int i = 0; i < count; i++)
            {
                Color c = colors[i];
                int r = c.R / Constants.ColorChannelMulti;
                int g = c.G / Constants.ColorChannelMulti;
                int b = c.B / Constants.ColorChannelMulti;

                ushort gbaColor = (ushort)((b << Constants.BlueShift) | (g << Constants.GreenShift) | (r << Constants.RedShift));
                paletteData[i * Constants.BytesPerColor] = (byte)(gbaColor & Constants.ByteMask);
                paletteData[i * Constants.BytesPerColor + 1] = (byte)((gbaColor >> Constants.BitsPerByte) & Constants.ByteMask);
            }

            // compress if needed
            return isCompressed
                ? CompressLZ77(paletteData)
                : paletteData;
        }
    }
}
