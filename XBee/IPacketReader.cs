namespace XBee
{
    public delegate void FrameReceivedHandler(object sender, FrameReceivedEventArgs args);

    public interface IPacketReader
    {
        event FrameReceivedHandler FrameReceived;
        void ReceiveData(byte[] data);
        ApiVersion ApiVersion { get; set; }
    }
}
