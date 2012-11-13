using System;
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
}
