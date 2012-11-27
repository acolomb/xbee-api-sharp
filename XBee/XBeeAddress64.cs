using System;
using System.Linq;

namespace XBee
{
    public class XBeeAddress64 : XBeeAddress
    {
        public static XBeeAddress64 BROADCAST = new XBeeAddress64(0x000000000000FFFF);
        public static XBeeAddress64 ZNET_COORDINATOR = new XBeeAddress64(0);

        private readonly byte[] address;

        public XBeeAddress64(ulong address)
        {
            var addressLittleEndian = BitConverter.GetBytes(address);
            if (BitConverter.IsLittleEndian) Array.Reverse(addressLittleEndian);
            this.address = addressLittleEndian;
        }

		public XBeeAddress64(ushort high, ushort low)
		{
			address = new byte[8];

			var highArray = BitConverter.GetBytes(high);
			if (BitConverter.IsLittleEndian) Array.Reverse(highArray);
			highArray.CopyTo(address, 0);

			var lowArray = BitConverter.GetBytes(low);
			if (BitConverter.IsLittleEndian) Array.Reverse(lowArray);
			lowArray.CopyTo(address, 4);
		}

        public XBeeAddress64(XBeeAddress16 shortAddress)
        {
            var source = shortAddress.GetAddress();
            address = new byte[8];
            source.CopyTo(address, address.Length - source.Length);
        }

		public override byte[] GetAddress()
        {
            return address;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;

            if ((obj == null) || (typeof(XBeeAddress64) != obj.GetType()))
                return false;

            var addr = (XBeeAddress64) obj;

            return GetAddress().SequenceEqual(addr.GetAddress());
        }

        public bool Equals(XBeeAddress16 obj)
        {
            var shortAddress = obj.GetAddress();
            if (address.Take(address.Length - shortAddress.Length).All(element => element == 0)) {
                return address.Skip(address.Length - shortAddress.Length).SequenceEqual(shortAddress);
            } else {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return address.GetHashCode();
        }
    }
}
