using XBee.Utils;

namespace XBee.Frames.ATCommands
{
    public class ATVersionAttribute : EnumAttribute
    {
        private bool series1Api;
        private bool series2Api;
        
        public ATVersionAttribute(bool api802_15_4, bool apiZB)
        {
            Api802_15_4 = api802_15_4;
            ApiZB = apiZB;
        }
        
        public bool Api802_15_4 {
            get { return series1Api; }
            private set { series1Api = value; }
        }
        
        public bool ApiZB {
            get { return series2Api; }
            private set { series2Api = value; }
        }
        
        public bool ApiAll {
            get { return series1Api && series2Api; }
            private set { series1Api = series2Api = value; }
        }
    }

    public enum ATVersion
    {
        [ATVersion(true, false)]
        S1  = 0x01,
        [ATVersion(false, true)]
        S2  = 0x02,
        [ATVersion(true, true)]
        All = 0xff,
    }
}

