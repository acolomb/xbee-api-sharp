using System;
using System.IO;
using XBee.Frames.ATCommands;
using XBee.Utils;

namespace XBee.Frames
{
    public class ATCommand : XBeeFrame
    {
        protected readonly PacketParser parser;
        
        public AT Command { get; protected set; }
        private ATValue value;
        protected bool hasValue;
        public ATValue Value {
            get {
                return this.value;
            }
            set {
                this.value = value;
                if (value != null) hasValue = true;
            }
        }

        public ATCommand(PacketParser parser)
        {
            this.parser = parser;
            CommandId = XBeeAPICommandId.AT_COMMAND_REQUEST;
        }

        public ATCommand(AT atCommand)
        {
            this.Command = atCommand;
            CommandId = XBeeAPICommandId.AT_COMMAND_REQUEST;
        }

        public override byte[] ToByteArray()
        {
            var stream = new MemoryStream();

            stream.WriteByte((byte) CommandId);
            stream.WriteByte(FrameId);

            var cmd = ((ATAttribute) Command.GetAttr()).ATCommand.ToCharArray();
            stream.WriteByte((byte) cmd[0]);
            stream.WriteByte((byte) cmd[1]);

            if (hasValue) {
                var v = value.ToByteArray();
                stream.Write(v, 0, v.Length);
            }

            return stream.ToArray();
        }

        public override void Parse()
        {
            FrameId = (byte) parser.ReadByte();
            Command = parser.ReadATCommand();

            if (parser.HasMoreData()) {
                Console.WriteLine("TODO: has data!"); //FIXME
            }
        }
    }
}
