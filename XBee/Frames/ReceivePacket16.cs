namespace XBee.Frames
{
    public class ReceivePacket16 : ReceivePacket64
    {
        public ReceivePacket16(PacketParser parser) :
            base(parser)
        {
            CommandId = XBeeAPICommandId.RECEIVE_16_RESPONSE;
        }

        public override void Parse()
        {
            Source = new XBeeNode { Address16 = parser.ReadAddress16() };
            ParseData();
        }
    }
}
