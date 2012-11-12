using System;
using System.IO;
using System.Text;
using XBee.Frames.ATCommands;
using XBee.Exceptions;

namespace XBee
{
    public class PacketParser
    {
        private readonly MemoryStream packetStream;

        public PacketParser(byte[] packet)
        {
            packetStream = new MemoryStream(packet);
        }

        public PacketParser(MemoryStream packetStream)
        {
            this.packetStream = packetStream;
        }

        private byte[] ReadIntoBuffer<T>(int neededBytes)
        {
            var buffer = new byte[neededBytes];

            var dataLength = packetStream.Read(buffer, 0, buffer.Length);
            if (dataLength != buffer.Length)
                throw new XBeeProtocolException(String.Format("Only {0} instead of {1} bytes available to parse {2}.",
                                                              dataLength, neededBytes, typeof(T)));

            return buffer;
        }

        public XBeeAddress64 ReadAddress64()
        {
            var addr = ReadIntoBuffer<XBeeAddress64>(8);
            if (BitConverter.IsLittleEndian) Array.Reverse(addr);

            return new XBeeAddress64(BitConverter.ToUInt64(addr, 0));
        }

        public XBeeAddress16 ReadAddress16()
        {
            var addr = ReadIntoBuffer<XBeeAddress16>(2);
            if (BitConverter.IsLittleEndian) Array.Reverse(addr);

            return new XBeeAddress16(BitConverter.ToUInt16(addr, 0));
        }

        public AT ReadATCommand()
        {
            var cmd = ReadIntoBuffer<AT>(2);
  
            return ATUtil.Parse(Encoding.ASCII.GetString(cmd));
        }

        public int ReadByte()
        {
            var value = packetStream.ReadByte();
            if (value == -1)
                throw new XBeeProtocolException("Expected data byte not available.");

            return value;
        }

        public ushort ReadUInt16()
        {
            var value = ReadIntoBuffer<UInt16>(2);
            if (BitConverter.IsLittleEndian) Array.Reverse(value);

            return BitConverter.ToUInt16(value, 0);
        }

        public uint ReadUInt32()
        {
            var value = ReadIntoBuffer<UInt32>(4);
            if (BitConverter.IsLittleEndian) Array.Reverse(value);

            return BitConverter.ToUInt32(value, 0);
        }

        public bool HasMoreData()
        {
            return (packetStream.Position != packetStream.Length);
        }

        public byte[] ReadData()
        {
            var data = ReadIntoBuffer<byte[]>((int) (packetStream.Length - packetStream.Position));

            return data;
        }

        public string ReadString()
        {
            var sb = new StringBuilder();
            char b;
            while((b = (char)packetStream.ReadByte()) != 0x00) {
                sb.Append(b);
            }
            return sb.ToString();
        }
    }
}