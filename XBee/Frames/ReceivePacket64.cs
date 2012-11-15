namespace XBee.Frames
{
    public class ReceivePacket64 : XBeeFrame
    {
        protected readonly PacketParser parser;

        public XBeeNode Source { get; protected set; }
        public byte SignalStrength { get; protected set; }
        public ReceiveOptionsType ReceiveOptions { get; protected set; }
        public byte[] Data { get; protected set; }

        public ReceivePacket64(PacketParser parser)
        {
            this.parser = parser;
            CommandId = XBeeAPICommandId.RECEIVE_64_RESPONSE;
        }

        public override byte[] ToByteArray()
        {
            return new byte[] { };
        }

        public override void Parse()
        {
            Source = new XBeeNode { Address64 = parser.ReadAddress64() };
            ParseData();
        }

        protected void ParseData()
        {
            SignalStrength = (byte) parser.ReadByte();
            ReceiveOptions = (ReceiveOptionsType) parser.ReadByte();
            Data = parser.ReadData();
        }
    }
}
