using System;
using System.IO;

namespace XBee.Frames
{
    public class TransmitRequest64 : XBeeFrame
    {
        [Flags]
        public enum OptionValues : byte
        {
            DisableAck      = 0x01,
			BroadcastPanId  = 0x04,
            ExtendedTimeout = 0x40,
        }

        private readonly XBeeNode destination;
        private byte[] rfData;

        public OptionValues Options { get; set; }

        public override ApiVersion SupportedApiVersions {
            get { return ApiVersion.S1; }
        }
        
        public TransmitRequest64(XBeeNode destination)
        {
            CommandId = XBeeAPICommandId.TX_REQUEST_64;
            this.destination = destination;
            Options = 0;
            rfData = null;
        }

        public void SetRFData(byte[] rfData)
        {
            this.rfData = rfData;
        }

        public override byte[] ToByteArray()
        {
            var stream = new MemoryStream();

            stream.WriteByte((byte)CommandId);
            stream.WriteByte(FrameId);

            WriteAddress(stream, destination);

            stream.WriteByte((byte)Options);

            if (rfData != null) {
                stream.Write(rfData, 0, rfData.Length);
            }

            return stream.ToArray();
        }

        protected virtual void WriteAddress(MemoryStream stream, XBeeNode dest)
        {
            stream.Write(dest.Address64.GetAddress(), 0, 8);
        }

        public override void Parse()
        {
            throw new NotImplementedException();//FIXME
        }
    }
}
