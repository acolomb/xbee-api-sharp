using System;
using System.IO;

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

        public static MemoryStream UnescapeData(byte[] data)
        {
            var unescapeNext = false;
            var escapedData = new MemoryStream();
            foreach (var b in data) {
                if (XBeeEscapeCharacters.IsSpecialByte(b)) {
                    if (b == (byte) XBeeSpecialBytes.EscapeByte) {
                        unescapeNext = true;
                        continue;
                    }
                    if (b == (byte) XBeeSpecialBytes.StartByte) {
                        continue;
                    }
                }
                
                if (unescapeNext) {
                    escapedData.WriteByte(XBeeEscapeCharacters.EscapeByte(b));
                    unescapeNext = false;
                } else {
                    escapedData.WriteByte(b);
                }
            }
            return escapedData;
        }
    }
}

