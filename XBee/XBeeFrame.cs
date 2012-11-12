namespace XBee
{
    public abstract class XBeeFrame
    {
        public XBeeAPICommandId CommandId { get; protected set; }
        public byte FrameId { get; set; }

        public abstract byte[] ToByteArray();
        public abstract void Parse();
    }
}
