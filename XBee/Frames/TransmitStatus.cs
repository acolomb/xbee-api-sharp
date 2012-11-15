using System;

namespace XBee.Frames
{
    public class TransmitStatus : XBeeFrame
    {
        [Flags]
        public enum StatusType
        {
            Success     = 0x00,
            NoAck       = 0x01,
            CCAFailure  = 0x02,
            Purged      = 0x04,
        }

        private readonly PacketParser parser;

        public StatusType Status;

        public override ApiVersion SupportedApiVersions {
            get { return ApiVersion.S1; }
        }
        
        public TransmitStatus(PacketParser parser)
        {
            this.parser = parser;
            CommandId = XBeeAPICommandId.ZIGBEE_TRANSMIT_STATUS_RESPONSE;
        }

        public override byte[] ToByteArray()
        {
            return new byte[] { };
        }

        public override void Parse()
        {
            FrameId = (byte) parser.ReadByte();
            Status = (StatusType) parser.ReadByte();
        }
    }
}
