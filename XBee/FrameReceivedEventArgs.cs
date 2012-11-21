using System;

namespace XBee
{
    public class FrameReceivedEventArgs : EventArgs
    {
        public XBeeFrame Response { get; private set; }

        public FrameReceivedEventArgs(XBeeFrame response)
        {
            Response = response;
        }
    }
}