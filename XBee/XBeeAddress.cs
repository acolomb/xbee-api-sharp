using System;
using System.Text;
using XBee.Utils;

namespace XBee
{
    public abstract class XBeeAddress : IFormattable
    {
        public abstract byte[] GetAddress();

        public override string ToString()
        {
            return ByteUtils.ToBase16(GetAddress());
        }

        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            var sb = new StringBuilder();
            foreach (var b in GetAddress()) {
                sb.Append(b.ToString(format, formatProvider));
            }
            return sb.ToString();
        }
    }
}
