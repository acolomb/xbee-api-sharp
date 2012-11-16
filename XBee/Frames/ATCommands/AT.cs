using System;
using XBee.Utils;

namespace XBee.Frames.ATCommands
{
    public class ATAttribute : EnumAttribute
    {
        public ATAttribute(string atCommand, string description, ATValueType type)
        {
            ATCommand = atCommand;
            Description = description;
            ApiVersion = ApiVersion.All;
            ReturnValueType = type;
        }

        public ATAttribute(string atCommand, string description, ApiVersion version, ATValueType type)
        {
            ATCommand = atCommand;
            Description = description;
            ApiVersion = version;
            ReturnValueType = type;
        }

        public ATAttribute(string atCommand, string description, ATValueType type, ulong maxValue)
        {
            ATCommand = atCommand;
            Description = description;
            ApiVersion = ApiVersion.All;
            ReturnValueType = type;
            MaximumValue = maxValue;
        }

        public ATAttribute(string atCommand, string description, ApiVersion version, ATValueType type, ulong maxValue)
        {
            ATCommand = atCommand;
            Description = description;
            ApiVersion = version;
            ReturnValueType = type;
            MaximumValue = maxValue;
        }

        public string ATCommand { get; private set; }
        public string Description { get; private set; }
        public ApiVersion ApiVersion { get; private set; }
        public ATValueType ReturnValueType { get; private set; }
        public ulong MaximumValue { get; private set; }
    }



    public class ATUtil
    {
        public static AT Parse(string value, ApiVersion apiVersion = ApiVersion.Unknown)
        {
            var atCommands = (AT[])Enum.GetValues(typeof(AT));
            var cmd = Array.Find(atCommands, at => {
                var attribute = (ATAttribute) at.GetAttr();
                return (attribute.ATCommand == value)
                    && (attribute.ApiVersion & apiVersion) == apiVersion; });

            if (cmd == 0)
                return AT.Unknown;

            return cmd;
        }
    }



    public enum AT
    {
        // Addressing
        [AT("DH", "Destination Address High", ATValueType.Number, 0xFFFFFFFF)]
        DestinationHigh = 0x10000,
        [AT("DL", "Destination Address Low", ATValueType.Number, 0xFFFFFFFF)]
        DestinationLow,
        [AT("MY", "16-bit Network Address", ATValueType.Number, 0xFFFE)]
        MyNetworkAddress,
        [AT("MP", "16-bit Parent Network Address", ApiVersion.S2, ATValueType.Number, 0xFFFE)]
        ParentAddress,
        [AT("NC", "Number of Remaining Children", ApiVersion.S2, ATValueType.Number)]
        RemainingChildren,
        [AT("SH", "Serial Number High", ATValueType.Number, 0xFFFFFFFF)]
        SerialNumberHigh,
        [AT("SL", "Serial Number Low", ATValueType.Number, 0xFFFFFFFF)]
        SerialNumberLow,
        [AT("NI", "Node Identifier", ATValueType.NodeIdentifier, 20)]
        NodeIdentifier,
        [AT("SE", "Source Endpoint", ApiVersion.S2, ATValueType.Number, 0xFF)]
        SourceEndpoint,
        [AT("DE", "Destination Endpoint", ApiVersion.S2, ATValueType.Number, 0xFF)]
        DestinationEndpoint,
        [AT("CI", "Cluster Identifier", ApiVersion.S2, ATValueType.Number, 0xFFFF)]
        ClusterIdentifier,
        [AT("NP", "Maximum RF Payload Bytes", ApiVersion.S2, ATValueType.Number, 0xFFFF)]
        MaximumPayloadLength,
        [AT("DD", "Device Type Identifier", ApiVersion.S2, ATValueType.Number, 0xFFFFFFFF)]
        DeviceTypeIdentifier,

        // Networking
        [AT("CH", "Operating Channel", ATValueType.Number, 0x1A)]
        OperatingChannel,
        [AT("ID", "Extended PAN ID", ApiVersion.S2, ATValueType.Number, 0xFFFFFFFFFFFFFFFF)]
        ExtendedPanId,
        [AT("OP", "Operating Extended PAN ID", ApiVersion.S2, ATValueType.Number, 0xFFFFFFFFFFFFFFFF)]
        OperatingExtendedPanId,
        [AT("NH", "Maximum Unicast Hops", ApiVersion.S2, ATValueType.Number, 0xFF)]
        UnicastHops,
        [AT("BH", "Broadcast Hops", ApiVersion.S2, ATValueType.Number, 0x1E)]
        BroadcastHops,
        [AT("OI", "Operating 16-bit PAN ID", ApiVersion.S2, ATValueType.Number, 0xFFFF)]
        OperatingPanId,
        [AT("NT", "Node Discovery Timeout", ATValueType.Number, 0xFF)]
        NodeDiscoveryTimeout,
        [AT("NO", "Network Discovery options", ATValueType.Number, 0x03)]
        NetworkDiscoveryOptions,
        [AT("SC", "Scan Channels", ATValueType.Number, 0x7FFF)]
        ScanChannels,
        [AT("SD", "Scan Duration", ATValueType.Number, 0x07)]
        ScanDuration,
        [AT("ZS", "ZigBee Stack Profile", ApiVersion.S2, ATValueType.Number, 0x02)]
        ZigBeeStackProfile,
        [AT("NJ", "Node Join Time", ApiVersion.S2, ATValueType.Number, 0xFF)]
        NodeJoinTime,
        [AT("JV", "Channel Verification", ApiVersion.S2, ATValueType.Number)]
        ChannelVerification,
        [AT("NW", "Network Watchdog Timeout", ApiVersion.S2, ATValueType.Number, 0x64FF)]
        NetworkWatchdogTimeout,
        [AT("JN", "Join Notification", ApiVersion.S2, ATValueType.Number, 0x01)]
        JoinNotification,
        [AT("AR", "Aggregate Routing Notification", ApiVersion.S2, ATValueType.Number, 0xFF)]
        AggregateRoutingNotification,
        [AT("DJ", "Disable Joining", ApiVersion.S2, ATValueType.Number, 0x01)]
        DisableJoining,
        [AT("II", "Initial ID", ApiVersion.S2, ATValueType.Number, 0xFFFF)]
        InitialID,

        // Security
        [AT("EE", "Encryption Enable", ATValueType.Number, 0x01)]
        EncryptionEnable,
        [AT("EO", "Encryption Options", ApiVersion.S2, ATValueType.Number, 0xFF)]
        EncryptionOptions,
        [AT("NK", "Network Encryption Key", ApiVersion.S2, ATValueType.None)]
        NetworkEncryptionKey,
        [AT("KY", "Link Encryption Key", ATValueType.None)]
        LinkEncryptionKey,

        // RF Interfacing
        [AT("PL", "Power Level", ATValueType.Number, 0x04)]
        PowerLevel,
        [AT("PM", "Power Mode", ApiVersion.S2, ATValueType.Number, 0x01)]
        PowerMode,
        [AT("DB", "Received Signal Strength", ATValueType.Number)]
        ReceivedSignalStrength,
        [AT("PP", "Peak Power", ApiVersion.S2, ATValueType.Number, 0x12)]
        PeakPower,

        // Serial Interfacing
        [AT("AP", "API Enable", ATValueType.Number, 0x02)]
        ApiEnable,
        [AT("AO", "API Options", ApiVersion.S2, ATValueType.Number, 0x03)]
        ApiOptions,
        [AT("BD", "Interface Data Rate", ATValueType.Number, 0xE1000)]
        BaudRate,
        [AT("NB", "Serial Parity", ATValueType.Number, 0x03)]
        Parity,
        [AT("SB", "Stop Bits", ApiVersion.S2, ATValueType.Number, 0x01)]
        StopBits,
        [AT("RO", "Packetization Timeout", ATValueType.Number, 0xFF)]
        PacketizationTimeout,
        [AT("D7", "DIO7 Configuration", ATValueType.Number, 0x07)]
        DigitalIO7,
        [AT("D6", "DIO6 Configuration", ATValueType.Number, 0x05)]
        DigitalIO6,

        // I/O Commands
        [AT("IR", "IO Sample Rate", ATValueType.Number, 0xFFFF)]
        IOSampleRate,
        [AT("IC", "IO Digital Change Detection", ATValueType.Number, 0x0FFF)]
        IOChangeDetection,
        [AT("P0", "PWM0 Configuration", ATValueType.Number, 0x05)]
        Pwm0Configuration,
        [AT("P1", "DIO11 Configuration", ApiVersion.S2, ATValueType.Number, 0x05)]
        DigitalIO11,
        [AT("P2", "DIO12 Configuration", ApiVersion.S2, ATValueType.Number, 0x05)]
        DigitalIO12,
        [AT("P3", "DIO13 Configuration", ApiVersion.S2, ATValueType.Number, 0x05)]
        DigitalIO13,
        [AT("D0", "AD0/DIO0 Configuration", ATValueType.Number, 0x05)]
        DigitalIO0,
        [AT("D1", "AD1/DIO1 Configuration", ATValueType.Number, 0x05)]
        DigitalIO1,
        [AT("D2", "AD2/DIO2 Configuration", ATValueType.Number, 0x05)]
        DigitalIO2,
        [AT("D3", "AD3/DIO3 Configuration", ATValueType.Number, 0x05)]
        DigitalIO3,
        [AT("D4", "DIO4 Configuration", ATValueType.Number, 0x05)]
        DigitalIO4,
        [AT("D5", "DIO5 Configuration", ATValueType.Number, 0x05)]
        DigitalIO5,
        [AT("D8", "DIO8 Configuration", ATValueType.Number, 0x05)]
        DigitalIO8,
        [AT("LT", "Assoc LED Blink Time", ApiVersion.S2, ATValueType.Number, 0xFF)]
        LedBlinkTime,
        [AT("PR", "Pull-up Resistor", ATValueType.Number, 0x3FFF)]
        PullUpResistor,
        [AT("RP", "RSSI PWM Timer", ATValueType.Number, 0xFF)]
        RSSITimer,
        [AT("%V", "Supply Voltage", ApiVersion.S2, ATValueType.Number, 0xFFFF)]
        SupplyVoltage,
        [AT("V+", "Voltage Supply Monitoring", ApiVersion.S2, ATValueType.Number, 0xFFFF)]
        VoltageMonitoring,
        [AT("TP", "Reads the module temperature in Degrees Celsius", ApiVersion.S2, ATValueType.Number, 0xFFFF)]
        Temperature,

        // Diagnostics
        [AT("VR", "Firmware Version", ATValueType.Number, 0xFFFF)]
        FirmwareVersion,
        [AT("HV", "Hardware Version", ATValueType.Number, 0xFFFF)]
        HardwareVersion,
        [AT("AI", "Association Indication", ATValueType.Number, 0xFF)]
        AssociationIndication,

        // AT Command Options
        [AT("CT", "Command Mode Timeout", ATValueType.Number, 0x028F)]
        CommandModeTimeout,
        [AT("CN", "Exit Command Mode", ATValueType.None)]
        ExitCommandMode,
        [AT("GT", "Guard Times", ATValueType.Number, 0x0CE4)]
        GuardTimes,
        [AT("CC", "Command Sequence Character", ATValueType.Number, 0xFF)]
        CommandSequenceCharacter,
        
        // Sleep Commands
        [AT("SM", "Sleep Mode", ATValueType.Number, 0x05)]
        SleepMode,
        [AT("SN", "Number of Sleep Periods", ApiVersion.S2, ATValueType.Number, 0xFFFF)]
        NumberOfSleepPeriods,
        [AT("SP", "Sleep Period", ATValueType.Number, 0x0AF0)]
        SleepPeriod,
        [AT("ST", "Time Before Sleep", ATValueType.Number, 0xFFFE)]
        TimeBeforeSleep,
        [AT("SO", "Sleep Options", ATValueType.Number, 0x06)]
        SleepOptions,
        [AT("WH", "Wake Host", ApiVersion.S2, ATValueType.Number, 0xFFFF)]
        WakeHost,
        [AT("SI", "Sleep Immediately", ApiVersion.S2, ATValueType.None)]
        SleepImmediately,
        [AT("PO", "Polling Rate", ApiVersion.S2, ATValueType.Number, 0x3E8)]
        PollingRate,

        // Execution Commands
        [AT("AC", "Apply Changes", ATValueType.None)]
        ApplyChanges,
        [AT("WR", "Write", ATValueType.None)]
        Write,
        [AT("RE", "Restore Defaults", ATValueType.None)]
        RestoreDefaults,
        [AT("FR", "Software Reset", ATValueType.None)]
        SoftwareReset,
        [AT("NR", "Network Reset", ApiVersion.S2, ATValueType.Number, 0x01)]
        NetworkReset,
        [AT("CB", "Commissioning Pushbutton", ApiVersion.S2, ATValueType.None)]
        CommissioningPushButton,
        [AT("ND", "Node Discover", ApiVersion.S2, ATValueType.NodeDiscoverZB)]
        NodeDiscover,
        [AT("DN", "Destination Node", ATValueType.Number)]
        DestinationNode,
        [AT("IS", "Force Sample", ATValueType.None)]
        ForceSample,
        [AT("1S", "XBee Sensor Sample", ApiVersion.S2, ATValueType.None)]
        SensorSample,

		// invalid value
        [AT("", "Unknown AT Command", ApiVersion.All, ATValueType.None)]
        Unknown = 0
    }
}
