using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PoochyEnabler.Helpers
{
    public static class ImageHelper
    {
        private const int LZ77HeaderSize = 0x4;
        private const int LZ77HeaderIdentifier = 0x10;
        private const int LZ77MaxDistance = 4096;
        private const int LZ77MaxLength = 18;
        private const int LZ77MinMatchLength = 3;
        private const int LZ77MinSafeDistance = 2;
        private const int LZ77CompressedUnitSize = 2;

        public static byte[] DecompressLZ77(byte[] romData, int baseOffset)
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

        public static byte[] CompressLZ77(byte[] imageData)
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
            while (result.Count % Constants.UIntSize != 0)
            {
                result.Add(0);
            }

            return result.ToArray();
        }

        private static (int distance, int length) FindLongestMatch(byte[] data, int pos)
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

        public static byte[] DecompressPalette(byte[] romData, int offset, bool isCompressed)
        {
            if (isCompressed)
            {
                return DecompressLZ77(romData, offset);
            }

            var paletteData = new byte[Constants.PalColorCount * Constants.BytesPerColor];
            Array.Copy(romData, offset, paletteData, 0, paletteData.Length);
            return paletteData;
        }

        public static byte[] CompressPalette(byte[] rawPaletteData, bool isCompressed)
        {
            return isCompressed ? CompressLZ77(rawPaletteData) : rawPaletteData;
        }

        public static Bitmap CreateBitmap(byte[] imageData, byte[] rawPalette, int width, int height, bool showBackColor)
        {
            var bmp = new Bitmap(width, height, PixelFormat.Format4bppIndexed);

            // set palette from raw bytes
            ColorPalette bmpPalette = bmp.Palette;
            int paletteCount = Math.Min(rawPalette.Length / Constants.BytesPerColor, Constants.PalColorCount);

            for (int i = 0; i < paletteCount; i++)
            {
                int byteIndex = i * Constants.BytesPerColor;
                if (byteIndex + 1 >= rawPalette.Length) break;

                // convert GBA color bytes to ARGB
                int temp = (rawPalette[byteIndex + 1] << Constants.BitsPerByte) | rawPalette[byteIndex];

                int r = ((temp & Constants.RedMask) >> Constants.RedShift) * Constants.ColorChannelMulti;
                int g = ((temp & Constants.GreenMask) >> Constants.GreenShift) * Constants.ColorChannelMulti;
                int b = ((temp & Constants.BlueMask) >> Constants.BlueShift) * Constants.ColorChannelMulti;

                bmpPalette.Entries[i] = (i == 0 && !showBackColor)
                    ? Color.FromArgb(0, r, g, b)
                    : Color.FromArgb(255, r, g, b);
            }

            // fill unused colors
            for (int i = paletteCount; i < Constants.PalColorCount; i++)
            {
                bmpPalette.Entries[i] = Color.Black;
            }

            bmp.Palette = bmpPalette;

            // write pixel data
            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format4bppIndexed);

            var pixels = new byte[bmpData.Stride * height];
            int dataIndex = 0;

            // write tiles
            for (int yTile = 0; yTile < height; yTile += Constants.TileSize)
            {
                for (int xTile = 0; xTile < width; xTile += Constants.TileSize)
                {
                    for (int yPixel = 0; yPixel < Constants.TileSize; yPixel++)
                    {
                        for (int xPixel = 0; xPixel < Constants.TileSize; xPixel += Constants.PixelsPerByte4Bpp)
                        {
                            if (dataIndex >= imageData.Length) break;

                            byte temp = imageData[dataIndex++];
                            int leftIndex = temp & Constants.NibbleMask;
                            int rightIndex = (temp >> Constants.NibbleShift) & Constants.NibbleMask;

                            int byteIndex = (yTile + yPixel) * bmpData.Stride + ((xTile + xPixel) / Constants.PixelsPerByte4Bpp);
                            pixels[byteIndex] = (byte)((leftIndex << Constants.Bpp4) | rightIndex);
                        }
                    }
                }
            }

            // unlock bitmap
            Marshal.Copy(pixels, 0, bmpData.Scan0, pixels.Length);
            bmp.UnlockBits(bmpData);

            return bmp;
        }

        public static bool ExtractImageAndPalette(
            Bitmap bmp,
            int expectedWidth,
            int expectedHeight,
            out byte[] imageData,
            out byte[] rawPalette)
        {
            imageData = null;
            rawPalette = null;

            // check size
            if (bmp.Width != expectedWidth || bmp.Height != expectedHeight)
            {
                MessageBox.Show(
                    $"Image size must be {expectedWidth}x{expectedHeight}.",
                    "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            // check format
            if (bmp.PixelFormat != PixelFormat.Format4bppIndexed)
            {
                MessageBox.Show(
                    "Use a 4bpp (16-color) indexed image.",
                    "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            // read palette and convert directly to GBA bytes
            ColorPalette pal = bmp.Palette;
            rawPalette = new byte[Constants.PalColorCount * Constants.BytesPerColor];

            for (int i = 0; i < Constants.PalColorCount; i++)
            {
                Color c = (i < pal.Entries.Length)
                    ? pal.Entries[i]
                    : Color.FromArgb(255, 0, 0, 0);

                int r = c.R / Constants.ColorChannelMulti;
                int g = c.G / Constants.ColorChannelMulti;
                int b = c.B / Constants.ColorChannelMulti;

                ushort gbaColor = (ushort)((b << Constants.BlueShift) | (g << Constants.GreenShift) | (r << Constants.RedShift));
                rawPalette[i * Constants.BytesPerColor] = (byte)(gbaColor & Constants.ByteMask);
                rawPalette[i * Constants.BytesPerColor + 1] = (byte)((gbaColor >> Constants.BitsPerByte) & Constants.ByteMask);
            }

            // read pixel data
            BitmapData bmpData = bmp.LockBits(
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format4bppIndexed);

            var pixels = new byte[bmpData.Stride * bmp.Height];
            Marshal.Copy(bmpData.Scan0, pixels, 0, pixels.Length);
            bmp.UnlockBits(bmpData);

            var dataList = new List<byte>();

            // read tiles
            for (int yTile = 0; yTile < expectedHeight; yTile += Constants.TileSize)
            {
                for (int xTile = 0; xTile < expectedWidth; xTile += Constants.TileSize)
                {
                    for (int yPixel = 0; yPixel < Constants.TileSize; yPixel++)
                    {
                        for (int xPixel = 0; xPixel < Constants.TileSize; xPixel += Constants.PixelsPerByte4Bpp)
                        {
                            int byteIndex = (yTile + yPixel) * bmpData.Stride + ((xTile + xPixel) / Constants.PixelsPerByte4Bpp);
                            byte pixelByte = pixels[byteIndex];

                            int p1 = (pixelByte >> Constants.Bpp4) & Constants.NibbleMask;
                            int p2 = pixelByte & Constants.NibbleMask;
                            dataList.Add((byte)((p2 << Constants.NibbleShift) | p1));
                        }
                    }
                }
            }

            imageData = dataList.ToArray();
            return true;
        }

        public static void ExportIndexedImage(Bitmap bmp, string filePath)
        {
            if (bmp == null) return;

            using (var exportBmp = (Bitmap)bmp.Clone())
            {
                // remove transparency
                ColorPalette pal = exportBmp.Palette;
                for (int i = 0; i < pal.Entries.Length; i++)
                {
                    Color e = pal.Entries[i];
                    pal.Entries[i] = Color.FromArgb(255, e.R, e.G, e.B);
                }
                exportBmp.Palette = pal;

                // save image
                var format = Path.GetExtension(filePath).ToLower() == ".bmp"
                    ? ImageFormat.Bmp
                    : ImageFormat.Png;

                exportBmp.Save(filePath, format);
            }
        }

        public static Bitmap ScaleBitmap(Bitmap originalBmp, int scaleFactor = Constants.DefaultScale)
        {
            int newWidth = originalBmp.Width * scaleFactor;
            int newHeight = originalBmp.Height * scaleFactor;
            var scaledBmp = new Bitmap(newWidth, newHeight);

            // draw scaled image
            using (Graphics g = Graphics.FromImage(scaledBmp))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.DrawImage(originalBmp, new Rectangle(0, 0, newWidth, newHeight));
            }

            return scaledBmp;
        }
    }
}
