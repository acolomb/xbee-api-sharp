using System;
using System.Threading;
using XBee.Frames;
using XBee.Frames.ATCommands;

namespace XBee.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var bee = new XBee {
                ApiType = ApiTypeValue.Enabled,
                ApiVersion = ApiVersion.S2
            };
            bee.SetConnection(new SerialConnection("COM4", 9600));

            XBeeFrame frame;

            var request = new ATCommand(AT.ApiEnable) { FrameId = 1 };
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                var value = (ATLongValue) atResponse.Value;
                Console.WriteLine("{0} status: {1}, result: {2}",
                                  atResponse.Command, atResponse.CommandStatus,
                                  value == null ? "<none>" : value.Value.ToString());
            }

            request = new ATCommand(AT.BaudRate) { FrameId = 1 };
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                var value = (ATLongValue) atResponse.Value;
                Console.WriteLine("{0} status: {1}, result: {2}",
                                  atResponse.Command, atResponse.CommandStatus,
                                  value == null ? "<none>" : value.Value.ToString());
            }

            request = new ATCommand(AT.MaximumPayloadLength) { FrameId = 1 };
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                var value = (ATLongValue) atResponse.Value;
                Console.WriteLine("{0} status: {1}, result: {2}",
                                  atResponse.Command, atResponse.CommandStatus,
                                  value == null ? "<none>" : value.Value.ToString());
            }

            request = new ATCommand(AT.FirmwareVersion) { FrameId = 1 };
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                var value = (ATLongValue) atResponse.Value;
                Console.WriteLine("{0} status: {1}, result: {2}",
                                  atResponse.Command, atResponse.CommandStatus,
                                  value == null ? "<none>" : value.Value.ToString("X4"));
            }

            request = new ATCommand(AT.NodeDiscover) { FrameId = 1 };
            bee.Execute(request);

            while (true) {
                Thread.Sleep(100);
            }
        }
    }
}
