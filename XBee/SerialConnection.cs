using System;
using System.IO;
using System.IO.Ports;
using NLog;
using XBee.Utils;

namespace XBee
{
    public class SerialConnection : IXBeeConnection
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly SerialPort serialPort;
        private IPacketReader reader;

        public event EventHandler<ReceiveExceptionEventArgs> ReceiveException;

        public Handshake FlowControl {
            get { return serialPort.Handshake; }
            set { serialPort.Handshake = value; }
        }

        public bool IsOpen {
            get { return serialPort.IsOpen; }
        }

        public SerialConnection(string port, int baudRate)
        {
            serialPort = new SerialPort(port, baudRate);
            serialPort.DataReceived += ReceiveData;

            FlowControl = Handshake.RequestToSend;
        }

        private void ReceiveData(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort.BytesToRead > 0) try {
                reader.ReceiveStreamData(serialPort.BaseStream, serialPort.BytesToRead);
            } catch (Exception ex) {
                if (ReceiveException != null) ReceiveException(this, new ReceiveExceptionEventArgs(ex));
                else logger.Warn("No handler to notify about exception:\n{0}", ex.Message);
            }
        }

        public void Write(byte[] data)
        {
            logger.Debug("Sending data: [" + ByteUtils.ToBase16(data) + "]");
            serialPort.Write(data, 0, data.Length);
        }

        public Stream GetStream()
        {
            return serialPort.BaseStream;
        }

        public void Open()
        {
            serialPort.Open();
        }

        public void Close()
        {
            lock (serialPort) {
                serialPort.Close();
            }
        }

        public void SetPacketReader(IPacketReader reader)
        {
            this.reader = reader;
        }
    }
}
