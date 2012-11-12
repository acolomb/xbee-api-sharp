using System;
using System.IO;
using XBee.Frames.ATCommands;
using XBee.Utils;

namespace XBee.Frames
{
    public class RemoteATCommand : ATCommand
    {
        public XBeeNode Destination { get; set; }
        public byte RemoteOptions { get; set; }


        public RemoteATCommand(PacketParser parser) :
            base(parser)
        {
            CommandId = XBeeAPICommandId.REMOTE_AT_COMMAND_REQUEST;
        }

        public RemoteATCommand(AT command, XBeeNode destination) :
            base(command)
        {
            CommandId = XBeeAPICommandId.REMOTE_AT_COMMAND_REQUEST;
            Command = command;
            Destination = destination;
        }


        public override byte[] ToByteArray()
        {
            var stream = new MemoryStream();

            stream.WriteByte((byte)CommandId);
            stream.WriteByte(FrameId);

            stream.Write(Destination.Address64.GetAddress(), 0, 8);
            stream.Write(Destination.Address16.GetAddress(), 0, 2);

            stream.WriteByte(RemoteOptions);

            var cmd = ((ATAttribute)Command.GetAttr()).ATCommand.ToCharArray();
            stream.WriteByte((byte)cmd[0]);
            stream.WriteByte((byte)cmd[1]);

            if (hasValue) {
                var v = Value.ToByteArray();
                stream.Write(v, 0, v.Length);
            }

            return stream.ToArray();
        }

        public override void Parse()
        {
            FrameId = (byte) parser.ReadByte();

            Destination = new XBeeNode { Address64 = parser.ReadAddress64(), Address16 = parser.ReadAddress16() };

            RemoteOptions = (byte) parser.ReadByte();
            Command = parser.ReadATCommand();

            if (parser.HasMoreData()) {
                Console.WriteLine("TODO: has data!"); //FIXME
            }
        }
    }
}
