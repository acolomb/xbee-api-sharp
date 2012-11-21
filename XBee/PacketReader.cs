using System.IO;
using System.Linq;
using NLog;
using XBee.Exceptions;
using XBee.Utils;

namespace XBee
{
    public class PacketReader : IPacketReader
    {
        private enum ReaderState
        {
            Idle,
            LengthMSB,
            LengthLSB,
            Payload,
        }

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public event FrameReceivedHandler FrameReceived;

        private ReaderState state = ReaderState.Idle;
        private MemoryStream packetStream = new MemoryStream();
        private uint packetLength = 0;

        public ApiVersion ApiVersion { get; set; }

        public void ReceiveData(byte[] data)
        {
            var inputStream = new MemoryStream(data);
            CopyAndProcessData(inputStream);
        }

        private void CopyAndProcessData(Stream inputStream)
        {
            int b;
            while ((b = inputStream.ReadByte()) != -1) {
                switch (state) {
                case ReaderState.Idle:
                    if (b == (int) XBeeSpecialBytes.StartByte) {
                        packetStream = new MemoryStream();
                        state = ReaderState.LengthMSB;
                    } else {
                        logger.Info("Ignoring byte {0:X2} in packet reader state {1}.", b, state);
                    }
                    continue;   //don't store this byte in the output packet

                case ReaderState.LengthMSB:
                    WriteByte((byte) b);
                    packetLength = (uint) (b << 8);
                    state = ReaderState.LengthLSB;
                    break;
                
                case ReaderState.LengthLSB:
                    WriteByte((byte) b);
                    packetLength = (packetLength | (uint) b) + 3;
                    logger.Debug("Expecting packet length {0}.", packetLength);
                    state = ReaderState.Payload;
                    break;

                case ReaderState.Payload:
                    WriteByte((byte) b);
                    if (packetStream.Length == packetLength) {
                        ProcessReceivedData();
                        state = ReaderState.Idle;
                    }
                    break;
                }
            }
        }


        protected virtual void WriteByte(byte b)
        {
            packetStream.WriteByte((byte) b);
        }

        protected void ProcessReceivedData()
        {
            logger.Debug("API frame complete: [" + ByteUtils.ToBase16(packetStream.ToArray()) + "]");
            try {
                var frame = XBeePacketUnmarshaler.Unmarshal(packetStream.ToArray(), ApiVersion);
                packetLength = 0;
                if (FrameReceived != null)
                    FrameReceived.Invoke(this, new FrameReceivedEventArgs(frame));
            } catch (XBeeFrameException ex) {
                throw new XBeeException("Unable to unmarshal packet.", ex);
            }
        }
    }
}
