using System;
using System.IO;
using XBee.Exceptions;

namespace XBee
{
    public class XBeePacket
    {
        private readonly byte[] frameData;
        private MemoryStream packetStream = new MemoryStream();

        public byte[] Data {
            get { return packetStream.ToArray(); }
            private set { packetStream = new MemoryStream(value); }
        }

        public XBeePacket(XBeeFrame frame, ApiVersion apiVersion = ApiVersion.Unknown)
        {
            frame.UseApiVersion(apiVersion);
            frameData = frame.ToByteArray();
        }

        public XBeePacket(byte[] frameData)
        {
            this.frameData = frameData;
        }

        public void Assemble()
        {
            packetStream = new MemoryStream();

            packetStream.WriteByte((byte) XBeeSpecialBytes.StartByte);

            var packetLength = BitConverter.GetBytes((ushort)frameData.Length);
            var firstOffset = BitConverter.IsLittleEndian ? 1 : 0;
            WriteByte(packetLength[firstOffset]);
            WriteByte(packetLength[firstOffset ^ 1]);

            foreach (var b in frameData) {
                WriteByte(b);
            }

            WriteByte(XBeeChecksum.Calculate(frameData));
        }

        protected virtual void WriteByte(byte b)
        {
            packetStream.WriteByte((byte) b);
        }
    }
}
