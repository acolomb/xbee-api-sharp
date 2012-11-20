using System;
using XBee.Exceptions;

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

        public virtual void UseApiVersion(ApiVersion requested)
        {
            if (! TestApiVersion(requested))
                throw new XBeeFrameException(String.Format("Frame type {0} not supported by requested API version {1}.",
                                                           GetType(), requested));
        }

        public abstract void Parse();

        protected bool TestApiVersion(ApiVersion requested)
        {
            return SupportedApiVersions.HasFlag(requested);
        }
    }
}
