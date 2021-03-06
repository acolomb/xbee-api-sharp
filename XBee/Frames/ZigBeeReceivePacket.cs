namespace XBee.Frames
{
    public class ZigBeeReceivePacket : XBeeFrame
    {
        private readonly PacketParser parser;

        public XBeeNode Source { get; private set; }
        public ReceiveOptionsType ReceiveOptions { get; private set; }
        public byte[] Data { get; private set; }

        public override ApiVersion SupportedApiVersions {
            get { return ApiVersion.S2; }
        }
        
        public ZigBeeReceivePacket(PacketParser parser)
        {
            this.parser = parser;
            CommandId = XBeeAPICommandId.ZIGBEE_RX_RESPONSE;
        }

        public override byte[] ToByteArray()
        {
            return new byte[] { };
        }

        public override void Parse()
        {
            Source = new XBeeNode { Address64 = parser.ReadAddress64(), Address16 = parser.ReadAddress16() };
            ReceiveOptions = (ReceiveOptionsType) parser.ReadByte();
            Data = parser.ReadData();
        }
    }
}
