using System;
using System.IO;
using XBee.Exceptions;

namespace XBee
{
    public class XBeePacket
    {
        private readonly byte[] frameData;
        public byte[] Data { get; private set; }

        public XBeePacket(XBeeFrame frame, ApiVersion apiVersion = ApiVersion.Unknown)
        {
            if (! frame.UseApiVersion(apiVersion))
                throw new XBeeFrameException(String.Format("Unsupported frame type {0} for specified API version {1}.",
                                                           frame.GetType(), apiVersion));
            frameData = frame.ToByteArray();
        }

        public XBeePacket(byte[] frameData)
        {
            this.frameData = frameData;
        }

        public void Assemble()
        {
            var data = new MemoryStream();

            data.WriteByte((byte) XBeeSpecialBytes.StartByte);

            var packetLength = BitConverter.GetBytes((ushort)frameData.Length);
            if (BitConverter.IsLittleEndian) Array.Reverse(packetLength);

            data.WriteByte(packetLength[0]);
            data.WriteByte(packetLength[1]);

            data.Write(frameData, 0, frameData.Length);

            data.WriteByte(XBeeChecksum.Calculate(frameData));

            Data = data.ToArray();
        }

    }
}
