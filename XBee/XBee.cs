using System;
using System.Threading;
using NLog;
using XBee.Exceptions;

namespace XBee
{
    public enum ApiTypeValue : byte
    {
        Disabled = 0x00,
        Enabled = 0x01,
        EnabledWithEscape = 0x02,
        Unknown = 0xFF
    }

    public class XBee
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IXBeeConnection connection;

        private Thread receiveThread;
        private bool stopThread;

        private byte frameId = byte.MinValue; //FIXME

        private bool frameReceived = false;
        private XBeeFrame lastFrame = null;
        private IPacketReader reader;
        private ApiTypeValue apiType = ApiTypeValue.Enabled;
        private ApiVersion apiVersion;

        public XBee()
        {
            reader = PacketReaderFactory.GetReader(apiType);
        }

        public ApiTypeValue ApiType
        {
            get { return apiType; }
            set
            {
                apiType = value;
                reader = PacketReaderFactory.GetReader(apiType);
                reader.FrameReceived += FrameReceivedEvent;
            }
        }

        public ApiVersion ApiVersion
        {
            get { return apiVersion; }
            set { reader.ApiVersion = apiVersion = value; }
        }
        
        public void SetConnection(IXBeeConnection connection)
        {
            this.connection = connection;
            this.connection.SetPacketReader(reader);
            this.connection.Open();

            /*
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.Name = "Receive Data Thread";
            receiveThread.IsBackground = true;
            receiveThread.Start();
             */
        }

        public void Execute(XBeeFrame frame)
        {
            if (frame.FrameId != 0) {
                if (frameId == byte.MaxValue)
                    frameId = byte.MinValue;

                frame.FrameId = ++frameId; //FIXME
            }

            XBeePacket packet;
            switch (ApiType)
            {
            case ApiTypeValue.Disabled:
            case ApiTypeValue.Enabled:
                packet = new XBeePacket(frame, ApiVersion);
                break;
            case ApiTypeValue.EnabledWithEscape:
                packet = new XBeeEscapedPacket(frame, ApiVersion);
                break;
            default:
                throw new XBeeException("Invalid API Type");
            }

            packet.Assemble();
            if (connection == null)
                throw new XBeeException("No connection set for this XBee yet.");
            connection.Write(packet.Data);
        }

        public T ExecuteQuery<T>(XBeeFrame frame) where T : XBeeFrame
        {
            return (T) ExecuteQuery(frame);
        }

        public T ExecuteQuery<T>(XBeeFrame frame, int timeout) where T : XBeeFrame
        {
            return (T) ExecuteQuery(frame, timeout);
        }

        public XBeeFrame ExecuteQuery(XBeeFrame frame)
        {
            return ExecuteQuery(frame, 3000);
        }

        public XBeeFrame ExecuteQuery(XBeeFrame frame, int timeout)
        {
            if (frame.FrameId == 0)
                throw new XBeeFrameException("FrameId cannot be zero on a synchronous request.");

            lastFrame = null;
            frameReceived = false;

            lock (this) {
                Execute(frame);
            }

            while (!frameReceived && timeout > 0) {
                Thread.Sleep(10);
                timeout -= 10;
            }

            return lastFrame;
        }

        public void StopReceiveDataThread()
        {
            try {
                if (receiveThread != null) {
                    stopThread = true;
                    receiveThread.Join(2000);
                    receiveThread.Abort();
                    receiveThread.Join(2000);
                }
            } catch (Exception e) {
                logger.Info(e);
            } finally {
                receiveThread = null;
            }
        }

        public void FrameReceivedEvent(object sender, FrameReceivedArgs args)
        {
            frameReceived = true;
            lastFrame = args.Response;
            logger.Debug(args.Response);
        }

        private void ReceiveData()
        {
            try {
                while (!stopThread) {

                }
            } catch {
            }
        }
    }
}
