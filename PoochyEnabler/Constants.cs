namespace PoochyEnabler
{
    public static class Constants
    {
        public const int HexBase = 16;
        public const int BitsPerByte = 8;
        public const int CharPerByte = 2;
        public const int NibbleShift = 4;
        public const int NibbleMask = 0xF;
        public const int ByteMask = 0xFF;
        public const int UShortMask = 0xFFFF;
        public const uint UIntMask = 0xFFFFFFFFU;
        public const int ByteSize = 1;
        public const int UShortSize = 2;
        public const int UIntSize = 4;
        public const uint BaseAddr = 0x8000000U;
        public const uint AlignMask = 0xFFFFFFFCU;
        public const int InvalidOffset = -1;

        public const byte PaddingByte = 0x0;
        public const byte FreeSpaceByte = 0xFF;
        public const byte StrNewlineByte = 0xFE;
        public const byte StrTerminatorByte = 0xFF;

        public const int PalColorCount = 16;
        public const int BytesPerColor = 2;
        public const int ColorChannelMulti = 8;
        public const int RedShift = 0;
        public const int GreenShift = 5;
        public const int BlueShift = 10;
        public const int RedMask = 0x1F;
        public const int GreenMask = 0x3E0;
        public const int BlueMask = 0x7C00;
        public const int ArgbByteCount = 4;

        public const int TileSize = 8;
        public const int Bpp4 = 4;
        public const int PixelsPerByte4Bpp = BitsPerByte / Bpp4;
        public const int SpriteSize = 64;
        public const int DefaultScale = 2;

        public const string RomFileFilter = "ROM Files|*.gba";
        public const string RomFileTitle = "Select ROM";
        public const string ImageImportFilter = "Image Files (*.png;*.bmp)|*.png;*.bmp";
        public const string ImageExportFilter = "PNG Image (*.png)|*.png|BMP Image (*.bmp)|*.bmp";
        public const string BinImportExportFilter = "BIN Files (*.bin)|*.bin";
    }
}
