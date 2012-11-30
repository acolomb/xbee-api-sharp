using System.IO;

namespace XBee
{
    public delegate void FrameReceivedHandler(object sender, FrameReceivedEventArgs args);

    public interface IPacketReader
    {
        event FrameReceivedHandler FrameReceived;
        void ReceiveData(byte[] data);
        void ReceiveStreamData(Stream inputStream, int bytesToRead = -1);
        ApiVersion ApiVersion { get; set; }
    }
}
