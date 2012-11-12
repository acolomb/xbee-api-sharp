using System;
using System.Runtime.Serialization;

namespace XBee.Exceptions
{
    public class XBeeProtocolException : Exception
    {
        public XBeeProtocolException(String message)
            : base(message)
        { }

        public XBeeProtocolException(String message, Exception inner)
            : base(message, inner)
        { }

        protected XBeeProtocolException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }

    }
}
