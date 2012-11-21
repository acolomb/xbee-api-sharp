using System;
using System.Threading;
using XBee.Frames;
using XBee.Frames.ATCommands;
using XBee.Utils;

namespace XBee.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var bee = new XBee {
                ApiType = ApiTypeValue.EnabledWithEscape,
                ApiVersion = ApiVersion.S1
            };
            bee.SetConnection(new SerialConnection("COM10", 115200));

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

            request = new ATCommand(AT.MyNetworkAddress) { FrameId = 1 };
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

            request = new ATCommand(AT.DestinationNode) { FrameId = 1 };
            request.Value = new ATNodeIdentifierValue("TESTBOARD ONCHIP");
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                Console.WriteLine(String.Format("{0} status: {1}", atResponse.Command, atResponse.CommandStatus));
                value = atResponse.Value;
                if (value != null) Console.WriteLine(String.Format("Destination Node: {0}", ((ATNodeIdentifierValue) value).Value));
            }

            request = new ATCommand(AT.ActiveScan) { FrameId = 20 };
            //request = new ATCommand(AT.NodeDiscover) { FrameId = 1 };
            bee.Execute(request);

            do {
                Thread.Sleep(10000);
            } while (false);
        }
    }
}
