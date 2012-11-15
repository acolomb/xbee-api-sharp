namespace XBee
{
    public abstract class XBeeFrame
    {
        public XBeeAPICommandId CommandId { get; protected set; }
        public byte FrameId { get; set; }

        public abstract byte[] ToByteArray();

        public bool UseApiVersion(ApiVersion apiVersion)
        {
            // assume unchanged support for all API versions
            return true;
        }

        public abstract void Parse();
    }
}
