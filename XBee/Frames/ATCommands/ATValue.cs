using System;
using System.IO;
using System.Text;

namespace XBee.Frames.ATCommands
{
    public enum ATValueType
    {
        None,
        Number,
        String,
        HexString,
        NodeIdentifier,
        PanDescriptor,
        NodeDiscover,
    }



    public abstract class ATValue
    {
        public abstract ATValue FromByteArray(byte[] value);
        public abstract byte[] ToByteArray();
    }

    

    public class ATStringValue : ATValue
    {
        public string Value { get; set; }

        public ATStringValue()
        { }

        public ATStringValue(string v)
        {
            Value = v;
        }

        public override ATValue FromByteArray(byte[] value)
        {
            Value = Encoding.UTF8.GetString(value, 0, value.Length);
            return this;
        }

        public override byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(Value);
        }
    }

    

    public class ATLongValue : ATValue
    {
        public ulong Value { get; set; }

        public ATLongValue()
        { }

        public ATLongValue(ulong v)
        {
            Value = v;
        }

        public override ATValue FromByteArray(byte[] value)
        {
            Value = ToInt(value);
            return this;
        }

        private ulong ToInt(byte[] value)
        {
            switch (value.Length) {
                case 1:
                    return value[0];
                case 2:
                    return ((ulong) value[0] << 8 | value[1]);
                case 3:
                    var buffer = new byte[4] { 0, 0, 0, 0 };
                    Array.Copy(value, 0, buffer, 1, 3);
                    if (BitConverter.IsLittleEndian) Array.Reverse(buffer);
                    return BitConverter.ToUInt32(buffer, 0);
                case 4:
                    if (BitConverter.IsLittleEndian) Array.Reverse(value);
                    return BitConverter.ToUInt32(value, 0);
                case 8:
                    if (BitConverter.IsLittleEndian) Array.Reverse(value);
                    return BitConverter.ToUInt64(value, 0);
                default:
                    throw new InvalidCastException("Value has more bytes than a 64 bits integer.");
            }
        }

        public override byte[] ToByteArray()
        {
            byte[] longArray;
            if (Value <= 0xFF)
                longArray = new[] { (byte)Value };
            else if (Value <= 0xFFFF)
                longArray = BitConverter.GetBytes((ushort)Value);
            else if (Value <= 0xFFFFFFFF)
                longArray = BitConverter.GetBytes((uint)Value);
            else
                longArray = BitConverter.GetBytes(Value);
            if (BitConverter.IsLittleEndian) Array.Reverse(longArray);
            return longArray;
        }
    }



    public class ATNodeIdentifierValue : ATValue
    {
        public const int maxLength = 20;

        private char[] identifier;

        public string Value {
            get {
                return new string(identifier);
            }
            set {
                if (value.Length > maxLength) identifier = value.Substring(0, maxLength).ToCharArray();
                else identifier = value.ToCharArray();
            }
        }

        public ATNodeIdentifierValue()
        { }

        public ATNodeIdentifierValue(string v)
        {
            Value = v;
        }

        public override ATValue FromByteArray(byte[] value)
        {
            Value = Encoding.ASCII.GetString(value, 0, maxLength);
            return this;
        }

        public override byte[] ToByteArray()
        {
            return Encoding.ASCII.GetBytes(Value);
        }
    }



    public class ATNodeDiscoverValue : ATValue
    {
        private const int minLength = 2+4+4+1+1;

        public XBeeNode Source { get; set; }
        public ATLongValue SignalStrength { get; set; }
        public ATNodeIdentifierValue NodeIdentifier { get; set; }

        public ATNodeDiscoverValue()
        { }

        public override ATValue FromByteArray(byte[] value)
        {
            if (value.Length == 1 && value[0] == 0)	//end of records
                return new ATNodeDiscoverValue();

            if (value.Length < minLength)
                throw new InvalidCastException("Node Discover Response has too few bytes.");

            var parser = new PacketParser(value);

            Source = new XBeeNode { Address16 = parser.ReadAddress16(), Address64 = parser.ReadAddress64() };
            SignalStrength = new ATLongValue((ulong) parser.ReadByte());
            NodeIdentifier = new ATNodeIdentifierValue(parser.ReadString());
            return this;
        }

        public override byte[] ToByteArray()
        {
            var stream = new MemoryStream(minLength + NodeIdentifier.Value.Length);

            byte[] buffer;
            buffer = Source.Address16.GetAddress();
            stream.Write(buffer, 0, buffer.Length);

            stream.WriteByte(SignalStrength.ToByteArray()[0]);

            buffer = NodeIdentifier.ToByteArray();
            stream.Write(buffer, 0, buffer.Length);
            stream.WriteByte(0);

            return stream.ToArray();
        }
    }



    public class ATPanDescriptorValue : ATValue
    {
        private const int packetLength = 8+2+1+1+1+1+1+2+1+1+3;

        public enum CoordAddrMode {
            ShortAddress16Bit = 0x02,
            LongAddress64Bit = 0x03,
        };


        public XBeeAddress64 Coordinator { get; set; }
        public ATLongValue PanId { get; set; }
        public CoordAddrMode AddressMode { get; set; }
        public ATLongValue Channel { get; set; }
        public ATLongValue SecurityUse { get; set; }
        public ATLongValue ACLEntry { get; set; }
        public ATLongValue SecurityFailure { get; set; }
        public ATLongValue SuperFrameSpec { get; set; }
        public ATLongValue GtsPermit { get; set; }
        public ATLongValue SignalStrength { get; set; }
        public byte[] TimeStamp { get; set; }

        public ATPanDescriptorValue()
        { }

        public override ATValue FromByteArray(byte[] value)
        {
            if (value.Length < packetLength)
                throw new InvalidCastException("Pan Descriptor has too few bytes.");

            var parser = new PacketParser(value);

            Coordinator = parser.ReadAddress64();
            PanId = new ATLongValue(parser.ReadUInt16());
            AddressMode = (CoordAddrMode) parser.ReadByte();
            Channel = new ATLongValue((ulong) parser.ReadByte());
            SecurityUse = new ATLongValue((ulong) parser.ReadByte());
            ACLEntry = new ATLongValue((ulong) parser.ReadByte());
            SecurityFailure = new ATLongValue((ulong) parser.ReadByte());
            SuperFrameSpec = new ATLongValue(parser.ReadUInt16());
            GtsPermit = new ATLongValue((ulong) parser.ReadByte());
            SignalStrength = new ATLongValue((ulong) parser.ReadByte());
            TimeStamp = parser.ReadData();

            return this;
        }

        public override byte[] ToByteArray()
        {
            var stream = new MemoryStream(packetLength);

            byte[] buffer;
            buffer = Coordinator.GetAddress();
            stream.Write(buffer, 0, buffer.Length);

            buffer = PanId.ToByteArray();
            stream.Write(buffer, 0, buffer.Length);

            stream.WriteByte((byte) AddressMode);
            stream.WriteByte(Channel.ToByteArray()[0]);
            stream.WriteByte(SecurityUse.ToByteArray()[0]);
            stream.WriteByte(ACLEntry.ToByteArray()[0]);
            stream.WriteByte(SecurityFailure.ToByteArray()[0]);

            buffer = SuperFrameSpec.ToByteArray();
            stream.Write(buffer, 0, buffer.Length);

            stream.WriteByte(GtsPermit.ToByteArray()[0]);
            stream.WriteByte(SignalStrength.ToByteArray()[0]);

            stream.Write(TimeStamp, 0, TimeStamp.Length);

            return stream.ToArray();
        }
    }
}
