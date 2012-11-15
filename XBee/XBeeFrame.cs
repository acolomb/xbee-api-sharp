namespace XBee
{
    public abstract class XBeeFrame
    {
        public XBeeAPICommandId CommandId { get; protected set; }
        public byte FrameId { get; set; }
        public virtual ApiVersion SupportedApiVersions
        {
            get { return ApiVersion.All; } // assume unconditional support for all API versions
        }

        public abstract byte[] ToByteArray();

        public virtual bool UseApiVersion(ApiVersion requested)
        {
            return TestApiVersion(requested);
        }

        public abstract void Parse();

        protected bool TestApiVersion(ApiVersion requested)
        {
            return (SupportedApiVersions & requested) == requested;
        }
    }
}
