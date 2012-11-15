using System;
using System.IO;

namespace XBee.Frames
{
    public class TransmitRequest16 : TransmitRequest64
    {
        public TransmitRequest16(XBeeNode destination) :
            base(destination)
        {
            CommandId = XBeeAPICommandId.TRANSMIT_REQUEST_16;
        }

        protected override void WriteAddress(MemoryStream stream, XBeeNode dest)
        {
            stream.Write(dest.Address16.GetAddress(), 0, 2);
        }

        public override void Parse()
        {
            throw new NotImplementedException();//FIXME
        }
    }
}
