using System;
using System.Collections.Specialized;

namespace XBee.Frames
{
    public class ReceiveIOPacket64 : XBeeFrame
    {
        [Flags]
        public enum ChannelIndicatorType
        {
            D0      = 0x0001,
            D1      = 0x0002,
            D2      = 0x0004,
            D3      = 0x0008,
            D4      = 0x0010,
            D5      = 0x0020,
            D6      = 0x0040,
            D7      = 0x0080,
            D8      = 0x0100,
            Digital = 0x01ff,
            A0      = 0x0200,
            A1      = 0x0400,
            A2      = 0x0800,
            A3      = 0x1000,
            A4      = 0x2000,
            A5      = 0x4000,
            Analog  = 0x7e00,
            _na     = 0x8000,
        }

        protected readonly PacketParser parser;

        public XBeeNode Source { get; protected set; }
        public byte SignalStrength { get; protected set; }
        public ReceiveOptionsType ReceiveOptions { get; protected set; }

        public uint NumberOfSamples { get; protected set; }
        public ChannelIndicatorType ChannelIndicator { get; protected set; }
        public BitVector32[] DigitalSamples { get; protected set; }
        public UInt16[][] AnalogSamples { get; protected set; }

        public ReceiveIOPacket64(PacketParser parser)
        {
            this.parser = parser;
            CommandId = XBeeAPICommandId.RECEIVE_64_IO_RESPONSE;
        }

        public override byte[] ToByteArray()
        {
            return new byte[] { };
        }

        public override void Parse()
        {
            Source = new XBeeNode { Address64 = parser.ReadAddress64() };
            ParseData();
        }

        protected void ParseData()
        {
            ReceiveOptions = (ReceiveOptionsType) parser.ReadByte();
            NumberOfSamples = (uint) parser.ReadByte();

            ChannelIndicator = (ChannelIndicatorType) parser.ReadUInt16();
            if ((ChannelIndicator & ChannelIndicatorType.Analog) != 0) {
                AnalogSamples = new UInt16[NumberOfSamples][];
            }

            for (var sample = 0; sample < NumberOfSamples; ++sample) {
                if ((ChannelIndicator & ChannelIndicatorType.Digital) != 0) {
                    DigitalSamples[sample] = new BitVector32(parser.ReadUInt16());
                }

                for (uint ad = (uint) ChannelIndicatorType.A0, i = 0;
                     ad < (uint) ChannelIndicatorType.Analog;
                     ad <<= 1, ++i) {
                    if ((ChannelIndicator & (ChannelIndicatorType) ad) != 0) {
                        AnalogSamples[sample][i] = parser.ReadUInt16();
                    }
                }
            }
        }
    }
}
