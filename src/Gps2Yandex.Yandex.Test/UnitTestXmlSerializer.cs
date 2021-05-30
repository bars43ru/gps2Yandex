using Gps2Yandex.Yandex.Entities;
using Gps2Yandex.Yandex.Handlers;
using System;
using Xunit;

namespace Gps2Yandex.Yandex.Test
{
    public class UnitTestSendYandex
    {
        [Fact]
        public void TestXmlSerializerPoint()
        {

        }

        [Fact]
        public void TestXmlSerializerTrack()
        {

        }

        [Fact]
        public void TestXmlSerializerTracks()
        {
            const string waitValue = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<tracks clid=\"123\" />";
            Tracks tracks = new() { Clid = "123" };
            var resultValue = XmlSerializer.Serialize(tracks);
            if (resultValue != waitValue)
            {
                throw new Exception($"Error serialize value. Get `{resultValue}`, expected {resultValue}.");
            }
        }

        [Fact]
        public void TestXmlSerializerFullObject()
        {

        }
    }
}
