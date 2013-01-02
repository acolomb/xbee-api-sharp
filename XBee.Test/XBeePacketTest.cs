using NUnit.Framework;
using XBee.Frames;
using XBee.Exceptions;

namespace XBee.Test
{
    [TestFixture]
    class XBeePacketTest
    {
        [Test]
        public void TestXBeePacketWrongAPIVersionFrame()
        {
            var request = new CreateSourceRoute(new PacketParser(new System.IO.MemoryStream()));
            Assert.Throws(typeof(XBeeFrameException), delegate { new XBeePacket(request, ApiVersion.S1); });
        }

        [Test]
        public void ATCommandSampleTest()
        {
            byte[] atCommandSample = { 0x7E, 0x00, 0x04, 0x08, 0x52, 0x4E, 0x4A, 0x0D };
        }

        [Test]
        public void ATQueueCommandSampleTest()
        {
            byte[] atQueueCommandSample = { 0x7E, 0x00, 0x05, 0x09, 0x01, 0x42, 0x44, 0x07, 0x68 };
        }

        [Test]
        public void ZigBeeTransmitRequestAPI1Test()
        {
            byte[] zigBeeTransmitRequestAPI1Sample = { 0x7E, 0x00, 0x16, 0x10, 0x01, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x0A, 0x01, 0x27, 0xFF, 0xFE, 0x00, 0x00, 0x54, 0x78, 0x44, 0x61, 0x74, 0x61, 0x30, 0x41, 0x13 };
        }

        [Test]
        public void ZigBeeTransmitRequestAPI2Test()
        {
            byte[] zigBeeTransmitRequestAPI2Sample = { 0x7E, 0x00, 0x16, 0x10, 0x01, 0x00, 0x7D, 0x33, 0xA2, 0x00, 0x40, 0x0A, 0x01, 0x27, 0xFF, 0xFE, 0x00, 0x00, 0x54, 0x78, 0x44, 0x61, 0x74, 0x61, 0x30, 0x41, 0x7D, 0x33 };
        }

        [Test]
        public void ZigBeeTransmitRequestTest()
        {
            byte[] zigBeeTransmitRequestSample = { 0x7E, 0x00, 0x16, 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFE, 0x00, 0x00, 0x54, 0x78, 0x32, 0x43, 0x6F, 0x6F, 0x72, 0x64, 0xFC };
        }

        [Test]
        public void ExplicitAddressingCommandTest()
        {
            byte[] explicitAddressingCommandSample = { 0x7E, 0x00, 0x1A, 0x11, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFE, 0xA0, 0xA1, 0x15, 0x54, 0xC1, 0x05, 0x00, 0x00, 0x54, 0x78, 0x44, 0x61, 0x74, 0x61, 0x3A };
        }

        [Test]
        public void RemoteATCommandTest()
        {
            byte[] remoteATCommandSample = { 0x7E, 0x00, 0x10, 0x17, 0x01, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x40, 0x11, 0x22, 0xFF, 0xFE, 0x02, 0x42, 0x48, 0x01, 0xF5 };
        }

        [Test]
        public void CreateSourceRouteTest()
        {
            byte[] createSourceRouteSample = { 0x7E, 0x00, 0x14, 0x21, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x40, 0x11, 0x22, 0x33, 0x44, 0x00, 0x03, 0xEE, 0xFF, 0xDD, 0xAA, 0xBB, 0x01 };
        }

        [Test]
        public void ATCommandResponseTest()
        {
            byte[] atCommandResponseSample = { 0x7E, 0x00, 0x05, 0x88, 0x01, 0x42, 0x44, 0x00, 0xF0 };
        }

        [Test]
        public void ModemStatusTest()
        {
            byte[] modemStatusSample = { 0x7E, 0x00, 0x02, 0x8A, 0x06, 0x6F };
        }

        [Test]
        public void ZigBeeTransmitStatusTest()
        {
            byte[] zigBeeTransmitStatusSample = { 0x7E, 0x00, 0x07, 0x8B, 0x01, 0x7D, 0x84, 0x00, 0x00, 0x01, 0x71 };
        }

        [Test]
        public void ZigBeeReceivePacketTest()
        {
            byte[] zigBeeReceivePacketSample = { 0x7E, 0x00, 0x11, 0x90, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x52, 0x2B, 0xAA, 0x7D, 0x84, 0x01, 0x52, 0x78, 0x44, 0x61, 0x74, 0x61, 0x0D };
        }

        [Test]
        public void ZigBeeExplicitRXTest()
        {
            byte[] zigBeeExplicitRXSample = { 0x7E, 0x00, 0x18, 0x91, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x52, 0x2B, 0xAA, 0x7D, 0x84, 0xE0, 0xE0, 0x22, 0x11, 0xC1, 0x05, 0x02, 0x52, 0x78, 0x44, 0x61, 0x74, 0x61, 0x52 };
        }

        [Test]
        public void ZigBeeIODataSampleRXTest()
        {
            byte[] zigBeeIODataSampleRXSample = { 0x7E, 0x00, 0x14, 0x92, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x52, 0x2B, 0xAA, 0x7D, 0x84, 0x01, 0x01, 0x00, 0x1C, 0x02, 0x00, 0x14, 0x02, 0x25, 0xF5 };
        }

        [Test]
        public void SensorReadIndicatorTest()
        {
            byte[] sensorReadIndicatorSample = { 0x7E, 0x00, 0x17, 0x94, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x52, 0x2B, 0xAA, 0xDD, 0x6C, 0x01, 0x03, 0x00, 0x02, 0x00, 0xCE, 0x00, 0xEA, 0x00, 0x52, 0x01, 0x6A, 0x8B };
        }

        [Test]
        public void NodeIdentificationTest()
        {
            byte[] nodeIdentificationSample = { 0x7E, 0x00, 0x20, 0x95, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x52, 0x2B, 0xAA, 0x7D, 0x84, 0x02, 0x7D, 0x84, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x52, 0x2B, 0xAA, 0x20, 0x00, 0xFF, 0xFE, 0x01, 0x01, 0xC1, 0x05, 0x10, 0x1E, 0x1B };
        }

        [Test]
        public void RemoteCommandResponseTest()
        {
            byte[] remoteCommandResponseSample = { 0x7E, 0x00, 0x13, 0x97, 0x55, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x52, 0x2B, 0xAA, 0x7D, 0x84, 0x53, 0x4C, 0x00, 0x40, 0x52, 0x2B, 0xAA, 0xF0 };
        }

        [Test]
        public void OverAirUpdateStatusTest()
        {
            byte[] overAirUpdateStatusSample = { 0x7E, 0x00, 0x16, 0xA0, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x3E, 0x07, 0x50, 0x00, 0x00, 0x01, 0x52, 0x00, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x52, 0x2B, 0xAA, 0x66 };
        }

        [Test]
        public void RouteRecordIndicatorTest()
        {
            byte[] routeRecordIndicatorSample = { 0x7E, 0x00, 0x13, 0xA1, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x40, 0x11, 0x22, 0x33, 0x44, 0x01, 0x03, 0xEE, 0xFF, 0xCC, 0xDD, 0xAA, 0xBB, 0x80 };
        }

        [Test]
        public void ManyToOneRouteRequestTest()
        {
            byte[] manyToOneRouteRequestSample = { 0x7E, 0x00, 0x0C, 0xA3, 0x00, 0x13, 0xA2, 0x00, 0x40, 0x40, 0x11, 0x22, 0x00, 0x00, 0x00, 0xF4 };
        }
    }
}
