using System;
using System.Threading;
using XBee;
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

            var tracker = new XBeeResponseTracker();
            bee.FrameReceived += tracker.HandleFrameReceived;
            tracker.RegisterDefaultFrameHandler(HandleAnyFrame);

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
                                  value == null ? "<none>" : value.Value.ToString("X4"));
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
                var value = (ATNodeIdentifierValue) atResponse.Value;
                Console.WriteLine("{0} status: {1}, result: {2}",
                                  atResponse.Command, atResponse.CommandStatus,
                                  value == null ? "<none>" : value.Value);
            }

            request = new ATCommand(AT.ActiveScan) { FrameId = tracker.RegisterResponseHandler(HandleActiveScan) };
            bee.Execute(request);
            request = new ATCommand(AT.NodeDiscover) { FrameId = tracker.RegisterResponseHandler(HandleNodeDiscover) };
            bee.Execute(request);

            do {
                Thread.Sleep(10000);
            } while (false);
        }

        public static void HandleAnyFrame(object sender, FrameReceivedEventArgs args)
        {
            Console.WriteLine("Received {0} frame.", args.Response.CommandId);
        }

        public static void HandleActiveScan(object sender, FrameReceivedEventArgs args)
        {
            var atResponse = (ATCommandResponse) args.Response;
            var pan = (ATPanDescriptorValue) atResponse.Value;
            if (pan == null) {
                (sender as XBeeResponseTracker).UnregisterResponseHandler(atResponse.FrameId);
                Console.WriteLine("{0} status: {1}, end of records", atResponse.Command, atResponse.CommandStatus);
            } else {
                Console.WriteLine("{0} status: {1}, PAN ID {2}", atResponse.Command, atResponse.CommandStatus, pan.PanId);
            }
        }

        public static void HandleNodeDiscover(object sender, FrameReceivedEventArgs args)
        {
            var atResponse = (ATCommandResponse) args.Response;
            var node = (ATNodeDiscoverValue) atResponse.Value;
            if (node == null) {
                (sender as XBeeResponseTracker).UnregisterResponseHandler(atResponse.FrameId);
                Console.WriteLine("{0} status: {1}, end of records", atResponse.Command, atResponse.CommandStatus);
            } else {
                Console.WriteLine("{0} status: {1}, node {2}", atResponse.Command, atResponse.CommandStatus, node.NodeIdentifier.Value);
            }
        }
    }
}
