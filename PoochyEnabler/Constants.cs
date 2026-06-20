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
        public const uint AlignMask = 0xFFFFFFFCU;
        public const uint BaseAddr = 0x8000000U;
        public const int PaddingByte = 0x0;
        public const int FreeSpaceByte = 0xFF;
    }
}
