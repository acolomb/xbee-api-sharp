using System;
using System.IO;
using XBee.Exceptions;

namespace XBee
{
    public class XBeeEscapedPacket : XBeePacket
    {
        public XBeeEscapedPacket(XBeeFrame frame, ApiVersion apiVersion = ApiVersion.Unknown) :
            base(frame, apiVersion)
        {
        }

        public XBeeEscapedPacket(byte[] frameData) :
            base(frameData)
        {
        }

        protected override void WriteByte(byte b)
        {
            if (XBeeEscapeCharacters.IsSpecialByte(b)) {
                base.WriteByte((byte) XBeeSpecialBytes.EscapeByte);
                b = XBeeEscapeCharacters.EscapeByte(b);
            }
            base.WriteByte((byte) b);
        }
    }
}
