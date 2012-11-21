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
            var bee = new XBee {ApiType = ApiTypeValue.Enabled};
            bee.SetConnection(new SerialConnection("COM4", 9600));

            XBeeFrame frame;

            var request = new ATCommand(AT.ApiEnable) { FrameId = 1 };
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                var value = (ATLongValue) atResponse.Value;
                if (value != null) Console.WriteLine("API type: {0}", value.Value);
            }

            request = new ATCommand(AT.BaudRate) { FrameId = 1 };
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                var value = (ATLongValue) atResponse.Value;
                if (value != null) Console.WriteLine("Baud rate: {0}", value.Value);
            }

            request = new ATCommand(AT.MaximumPayloadLength) { FrameId = 1 };
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                var value = (ATLongValue) atResponse.Value;
                if (value != null) Console.WriteLine("Maximum Payload is: {0}", value.Value);
            }

            request = new ATCommand(AT.FirmwareVersion) { FrameId = 1 };
            frame = bee.ExecuteQuery(request, 1000);
            if (frame != null) {
                var atResponse = (ATCommandResponse) frame;
                var value = (ATLongValue) atResponse.Value;
                if (value != null) Console.WriteLine("Firmware Version: {0:X4}", value.Value);
            }

            request = new ATCommand(AT.NodeDiscover) { FrameId = 1 };
            bee.Execute(request);

            while (true) {
                Thread.Sleep(100);
            }
        }
    }
}
