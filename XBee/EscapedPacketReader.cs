using System;
using System.IO;

namespace XBee
{
    public class EscapedPacketReader : PacketReader
    {
        private bool unescapeNext = false;

        protected override void WriteByte(byte b)
        {
            if (unescapeNext) {
                b = XBeeEscapeCharacters.EscapeByte(b);
                unescapeNext = false;
            } else if (b == (byte) XBeeSpecialBytes.EscapeByte) {
                unescapeNext = true;
                return;
            }
            base.WriteByte(b);
        }

        public MemoryStream EscapeData(byte[] data)
        {
            var escapeNext = false;
            var escapedData = new MemoryStream();
            foreach (var b in data) {
                if (XBeeEscapeCharacters.IsSpecialByte(b)) {
                    if (b == (byte) XBeeSpecialBytes.EscapeByte) {
                        escapeNext = true;
                        continue;
                    }
                    if (b == (byte) XBeeSpecialBytes.StartByte) {
                        continue;
                    }
                }

                if (escapeNext) {
                    escapedData.WriteByte(XBeeEscapeCharacters.EscapeByte(b));
                    escapeNext = false;
                } else {
                    escapedData.WriteByte(b);
                }
            }
            return escapedData;
        }
    }
}