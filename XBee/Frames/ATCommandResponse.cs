using System;
using System.Text;
using XBee.Frames.ATCommands;
using XBee.Utils;

namespace XBee.Frames
{
    public class ATCommandResponse : XBeeFrame
    {
        public enum CommandStatusType
        {
            Ok = 0x00,
            Error = 0x01,
            InvalidCommand = 0x02,
            InvalidParameter = 0x03,
            TransmissionFailed = 0x04
        }
        
        protected readonly PacketParser parser;

        public AT Command { get; protected set; }
        public ATValue Value { get; protected set; }
        public CommandStatusType CommandStatus { get; protected set; }

        public ATCommandResponse(PacketParser parser)
        {
            this.parser = parser;
            CommandId = XBeeAPICommandId.AT_COMMAND_RESPONSE;
        }

        public ATCommandResponse()
        {
            CommandId = XBeeAPICommandId.AT_COMMAND_RESPONSE;
        }

        public override byte[] ToByteArray()
        {
            return new byte[] { };
        }

        public override void Parse()
        {
            FrameId = (byte)parser.ReadByte();
            Command = parser.ReadATCommand();
            CommandStatus = (CommandStatusType)parser.ReadByte();

            ParseValue();
        }

        protected void ParseValue()
        {
            if (Command == AT.NodeDiscover)
                ParseNetworkDiscovery();

            var type = ((ATAttribute) Command.GetAttr()).ReturnValueType;

            if (parser.HasMoreData()) {
                switch (type) {
                    case ATValueType.None:
                        break;
                    case ATValueType.Number:
                        var vData = parser.ReadData();
                        Value = new ATLongValue().FromByteArray(vData);
                        break;
                    case ATValueType.HexString:
                        var hexData = parser.ReadData();
                        Value = new ATStringValue(ByteUtils.ToBase16(hexData));
                        break;
                    case ATValueType.String:
                        var str = parser.ReadData();
                        Value = new ATStringValue(Encoding.UTF8.GetString(str));
                        break;
                }
            }
        }

        private void ParseNetworkDiscovery()
        {
            var source = new XBeeNode { Address16 = parser.ReadAddress16(), Address64 = parser.ReadAddress64() };
            var nodeIdentifier = parser.ReadString();
            var parentAddress = parser.ReadAddress16();
            var type = (NodeIdentification.DeviceType) parser.ReadByte();
            var status = parser.ReadByte();
            var profileId = parser.ReadUInt16();
            var manufacturerId = parser.ReadUInt16();

            Console.WriteLine(string.Format("source {0}, id {1}, status {2}", source.Address64, nodeIdentifier, status));
        }
    }
}
