using System;
using System.Collections.Specialized;

namespace XBee.Frames
{
    public class ReceiveIOPacket16 : ReceiveIOPacket64
    {
        public ReceiveIOPacket16(PacketParser parser) :
            base(parser)
        {
            CommandId = XBeeAPICommandId.RX_16_IO_RESPONSE;
        }

        public override void Parse()
        {
            Source = new XBeeNode { Address16 = parser.ReadAddress16() };
            ParseData();
        }
    }
}
