using System;

namespace XBee
{
    [Flags]
    public enum ApiVersion
    {
        Unknown = 0x00,
        S1      = 0x01,
        S2      = 0x02,
        All     = 0xff,
    }
}
