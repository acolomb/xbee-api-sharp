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

        private IPacketReader reader;
        private ApiTypeValue apiType = ApiTypeValue.Enabled;
        private ApiVersion apiVersion;
        private XBeeResponseTracker tracker;

        public event FrameReceivedHandler FrameReceived;

        public XBee()
        {
            InitPacketReader();
        }

        public XBee(ApiTypeValue type)
        {
            ApiType = type;
        }

        public ApiTypeValue ApiType
        {
            get { return apiType; }
            set {
                if (apiType != value) {
                    apiType = value;
                    InitPacketReader();
                }
            }
        }

        public ApiVersion ApiVersion
        {
            get { return apiVersion; }
            set { reader.ApiVersion = apiVersion = value; }
        }
        
        private void InitPacketReader()
        {
            reader = PacketReaderFactory.GetReader(apiType);
            reader.ApiVersion = ApiVersion;
            reader.FrameReceived += FrameReceivedEvent;
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

        public XBeeResponseTracker ResponseTracker {
            get {
                if (tracker == null) tracker = new XBeeResponseTracker();
                return tracker;
            }
        }

        public void Execute(XBeeFrame frame)
        {
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
            lock (connection) {
                connection.Write(packet.Data);
            }
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
            XBeeFrame response = null;
            var frameReceived = new AutoResetEvent(false);

            frame.FrameId = ResponseTracker.RegisterResponseHandler(
                delegate (XBeeResponseTracker sender, FrameReceivedEventArgs args) {
                    response = args.Response;
                    frameReceived.Set();
                });

            Execute(frame);
            frameReceived.WaitOne(timeout);
            ResponseTracker.UnregisterResponseHandler(frame.FrameId);

            return response;
        }

        public void ExecuteQueryAsync(XBeeFrame frame, ResponseReceivedHandler responseCallback)
        {
            frame.FrameId = ResponseTracker.RegisterResponseHandler(responseCallback);
            Execute(frame);
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

        public void FrameReceivedEvent(object sender, FrameReceivedEventArgs args)
        {
            logger.Debug(args.Response);

            if (args.Response.FrameId == XBeeResponseTracker.NoResponseFrameId
                || tracker == null) {
                if (FrameReceived != null)
                    FrameReceived.Invoke(this, args);
            } else {
                tracker.HandleFrameReceived(this, args);
            }
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
