using System;
using XBee.Frames.ATCommands;

namespace XBee.Frames
{
    public class ATQueueCommand : ATCommand
    {
        public ATQueueCommand(AT atCommand) : base(atCommand)
        {
            CommandId = XBeeAPICommandId.AT_COMMAND_QUEUE;
        }

        public override void Parse()
        {
            throw new NotImplementedException();//FIXME
        }
    }
}
