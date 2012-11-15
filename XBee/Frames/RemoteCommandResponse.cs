using System;
using System.Text;
using XBee.Frames.ATCommands;
using XBee.Utils;

namespace XBee.Frames
{
    public class RemoteCommandResponse : ATCommandResponse
    {
        public XBeeNode Source { get; private set; }

        public RemoteCommandResponse(PacketParser parser) :
            base(parser)
        {
            CommandId = XBeeAPICommandId.REMOTE_AT_COMMAND_RESPONSE;
        }

        public override byte[] ToByteArray()
        {
            return new byte[] { };
        }

        public override void Parse()
        {
            FrameId = (byte) parser.ReadByte();
            Source = new XBeeNode { Address64 = parser.ReadAddress64(), Address16 = parser.ReadAddress16() };
            Command = parser.ReadATCommand(ExpectedApi);
            CommandStatus = (CommandStatusType) parser.ReadByte();

            ParseValue();
        }
    }
}
