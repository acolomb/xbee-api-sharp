using System;

namespace XBee
{
    public class ReceiveExceptionEventArgs : EventArgs
    {
        public Exception Exception { get; private set; }

        public ReceiveExceptionEventArgs(Exception e)
        {
            Exception = e;
        }
    }
}