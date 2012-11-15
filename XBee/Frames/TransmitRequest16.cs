using System;
using System.IO;

namespace XBee.Frames
{
    public class TransmitRequest16 : XBeeFrame
    {
        [Flags]
        public enum OptionValues : byte
        {
            DisableAck = 0x01,
            ExtendedTimeout = 0x40,
        }

        private readonly XBeeNode destination;
        private byte[] rfData;

        public byte BroadcastRadius { get; set; }
        public OptionValues Options { get; set; }

        public TransmitRequest16(XBeeNode destination)
        {
            CommandId = XBeeAPICommandId.REQUEST_16;
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

            stream.Write(destination.Address16.GetAddress(), 0, 2);

            stream.WriteByte((byte)Options);

            if (rfData != null) {
                stream.Write(rfData, 0, rfData.Length);
            }

            return stream.ToArray();
        }

        public override void Parse()
        {
            throw new NotImplementedException();//FIXME
        }
    }
}
