namespace XBee
{
    public enum XBeeAPICommandId
    {
        TX_REQUEST_64                       = 0x00,
        TX_REQUEST_16                       = 0x01,
        AT_COMMAND                          = 0x08,
        AT_COMMAND_QUEUE                    = 0x09,   
        ZIGBEE_TX_REQUEST                   = 0x10,
        ZIGBEE_EXPLICIT_TX_REQUEST          = 0x11,
        REMOTE_AT_REQUEST                   = 0x17,
        FIXME_CREATE_SOURCE_ROUTE           = 0x21,
        RX_64_RESPONSE                      = 0x80,
        RX_16_RESPONSE                      = 0x81,
        RX_64_IO_RESPONSE                   = 0x82,
        RX_16_IO_RESPONSE                   = 0x83,    
        AT_RESPONSE                         = 0x88,    
        TX_STATUS_RESPONSE                  = 0x89,    
        MODEM_STATUS_RESPONSE               = 0x8A,
        ZIGBEE_TX_STATUS_RESPONSE           = 0x8B,
        ZIGBEE_RX_RESPONSE                  = 0x90,    
        ZIGBEE_EXPLICIT_RX_RESPONSE         = 0x91,
        ZIGBEE_IO_SAMPLE_RESPONSE           = 0x92,
        XBEE_SENSOR_READ_INDICATOR          = 0x94,
        ZIGBEE_NODE_IDENTIFICATION_RESPONSE = 0x95,
        REMOTE_AT_RESPONSE                  = 0x97,     
        FIRMWARE_UPDATE_STATUS              = 0xA0,
        ROUTE_RECORD_INDICATOR              = 0xA1,
        MANYTOONE_ROUTE_REQUEST_INDICATOR   = 0xA3,
        UNKNOWN                             = 0xFF,
    }
}
