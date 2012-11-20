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
    }
}