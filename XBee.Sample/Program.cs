using System;
using System.Threading;
using XBee;
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
            // Callback for unexpected response with unknown frame ID
            bee.ResponseTracker.UnexpectedResponse += HandleAnyFrame;
            // Callback for received frame without frame ID
            bee.FrameReceived += HandleAnyFrame;

            XBeeFrame frame;

            var request = new ATCommand(AT.ApiEnable);
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                var param = (ATLongValue) atResponse.Value;
                Console.WriteLine("{0} status: {1}, result: {2}",
                                  atResponse.Command, atResponse.CommandStatus,
                                  param == null ? "<none>" : param.Value.ToString());
            }

            request = new ATCommand(AT.BaudRate);
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                var param = (ATLongValue) atResponse.Value;
                Console.WriteLine("{0} status: {1}, result: {2}",
                                  atResponse.Command, atResponse.CommandStatus,
                                  param == null ? "<none>" : param.Value.ToString());
            }

            request = new ATCommand(AT.MaximumPayloadLength);
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                var param = (ATLongValue) atResponse.Value;
                Console.WriteLine("{0} status: {1}, result: {2}",
                                  atResponse.Command, atResponse.CommandStatus,
                                  param == null ? "<none>" : param.Value.ToString());
            }

            request = new ATCommand(AT.FirmwareVersion);
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                var param = (ATLongValue) atResponse.Value;
                Console.WriteLine("{0} status: {1}, result: {2}",
                                  atResponse.Command, atResponse.CommandStatus,
                                  param == null ? "<none>" : param.Value.ToString("X4"));
            }

            request = new ATCommand(AT.ActiveScan);
            bee.ExecuteQueryAsync(request, HandleActiveScan);
            request = new ATCommand(AT.NodeDiscover);
            bee.ExecuteQueryAsync(request, HandleNodeDiscover);

            while (true) {
                Thread.Sleep(100);
            }
        }

        public static void HandleAnyFrame(object sender, FrameReceivedEventArgs args)
        {
            Console.WriteLine("Received {0} frame.", args.Response.CommandId);
        }

        public static void HandleActiveScan(XBeeResponseTracker sender, FrameReceivedEventArgs args)
        {
            var atResponse = (ATCommandResponse) args.Response;
            var pan = (ATPanDescriptorValue) atResponse.Value;
            if (pan == null) {
                sender.UnregisterResponseHandler(atResponse.FrameId);
                Console.WriteLine("{0} status: {1}, end of records",
                                  atResponse.Command, atResponse.CommandStatus);
            } else {
                Console.WriteLine("{0} status: {1}, PAN ID {2:X4}",
                                  atResponse.Command, atResponse.CommandStatus,
                                  pan.PanId.Value);
            }
        }

        public static void HandleNodeDiscover(XBeeResponseTracker sender, FrameReceivedEventArgs args)
        {
            var atResponse = (ATCommandResponse) args.Response;
            var node = (ATNodeDiscoverValue) atResponse.Value;
            if (node == null) {
                sender.UnregisterResponseHandler(atResponse.FrameId);
                Console.WriteLine("{0} status: {1}, end of records",
                                  atResponse.Command, atResponse.CommandStatus);
            } else {
                Console.WriteLine("{0} status: {1}, node {2}",
                                  atResponse.Command, atResponse.CommandStatus,
                                  node.NodeIdentifier.Value);
            }
        }
    }
}
