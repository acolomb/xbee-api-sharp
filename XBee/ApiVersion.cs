using System;

namespace XBee
{
    [Flags]
    public enum ApiVersion
    {
        S1  = 0x01,
        S2  = 0x02,
        All = 0xff,
    }
}
