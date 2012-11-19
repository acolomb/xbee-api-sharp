using System;

namespace XBee
{
    public class XBeeEscapeCharacters
    {
        public static bool IsSpecialByte(byte b)
        {
            return Enum.IsDefined(typeof(XBeeSpecialBytes), b);
        }
        
        public static byte EscapeByte(byte b)
        {
            return (byte) (0x20 ^ b);
        }
    }
}

