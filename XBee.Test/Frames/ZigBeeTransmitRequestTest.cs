using NUnit.Framework;
using XBee.Frames;

namespace XBee.Test.Frames
{
    [TestFixture]
    class ZigBeeTransmitRequestTest
    {
        [Test]
        public void TestZigBeeTransmitRequestBroadcast()
        {
            var broadcast = new XBeeNode { Address16 = XBeeAddress16.ZNET_BROADCAST, Address64 = XBeeAddress64.BROADCAST };

            var frame = new ZigBeeTransmitRequest(broadcast);
            Assert.AreEqual(new byte[] { 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFE, 0x00, 0x00 }, frame.ToByteArray());
        }

        [Test]
        public void TestZigBeeTransmitRequestBroadcastFrameId()
        {
            var broadcast = new XBeeNode { Address16 = XBeeAddress16.ZNET_BROADCAST, Address64 = XBeeAddress64.BROADCAST };

            var frame = new ZigBeeTransmitRequest(broadcast) { FrameId = 1 };
            Assert.AreEqual(new byte[] { 0x10, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFE, 0x00, 0x00 }, frame.ToByteArray());
        }

        [Test]
        public void TestZigBeeTransmitRequestBroadcastRadius()
        {
            var broadcast = new XBeeNode { Address16 = XBeeAddress16.ZNET_BROADCAST, Address64 = XBeeAddress64.BROADCAST };

            var frame = new ZigBeeTransmitRequest(broadcast) { BroadcastRadius = 2 };
            Assert.AreEqual(new byte[] { 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFE, 0x02, 0x00 }, frame.ToByteArray());
        }

        [Test]
        public void TestZigBeeTransmitRequestBroadcastRadiusOptions()
        {
            var broadcast = new XBeeNode { Address16 = XBeeAddress16.ZNET_BROADCAST, Address64 = XBeeAddress64.BROADCAST };

            var frame = new ZigBeeTransmitRequest(broadcast) {
                BroadcastRadius = 2,
                Options =
                    ZigBeeTransmitRequest.OptionValues.DisableAck |
                    ZigBeeTransmitRequest.OptionValues.ExtendedTimeout
            };
            Assert.AreEqual(new byte[] { 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFE, 0x02, 0x41 }, frame.ToByteArray());
        }

        [Test]
        public void TestZigBeeTransmitRequestBroadcastRadiusOptionsData()
        {
            var broadcast = new XBeeNode { Address16 = XBeeAddress16.ZNET_BROADCAST, Address64 = XBeeAddress64.BROADCAST };

            var frame = new ZigBeeTransmitRequest(broadcast) {
                BroadcastRadius = 2,
                Options =
                    ZigBeeTransmitRequest.OptionValues.DisableAck |
                    ZigBeeTransmitRequest.OptionValues.ExtendedTimeout
            };
            frame.SetRFData(new byte[] { 0x11, 0x22, 0x33, 0x00 });
            Assert.AreEqual(new byte[] { 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFE, 0x02, 0x41, 0x11, 0x22, 0x33, 0x00 }, frame.ToByteArray());
        }
    }
}
