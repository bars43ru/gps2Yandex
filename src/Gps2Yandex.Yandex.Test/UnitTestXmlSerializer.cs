using Gps2Yandex.Yandex.Entities;
using Gps2Yandex.Yandex.Handlers;
using System;
using Xunit;

namespace Gps2Yandex.Yandex.Test
{
    public class UnitTestSendYandex
    {
        [Fact(DisplayName = "Xml формат Point")]
        public void TestXmlSerializerPoint()
        {
            const string waitValue = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                "<Point latitude=\"55.75363\" longitude=\"55.75363\" avg_speed=\"0\" direction=\"242\" time=\"10012009:142045\" />";
            Point point = new()
            {
                Latitude = 55.753630,
                Longitude = 55.753630,
                AvgSpeed = 0,
                Direction = 242,
                Time = "10012009:142045",
            };
            var resultValue = XmlSerializer.Serialize(point);
            if (resultValue != waitValue)
            {
                throw new Exception($"Error serialize value. Get `{resultValue}`, expected {resultValue}.");
            }
        }

        [Fact(DisplayName = "Xml формат Track")]
        public void TestXmlSerializerTrack()
        {
            const string waitValue =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                "<Track uuid=\"123456789\" category=\"n\" route=\"145\" vehicle_type=\"bus\">\r\n" +
                "  <point latitude=\"55.75363\" longitude=\"55.75363\" avg_speed=\"0\" direction=\"242\" time=\"10012009:142045\" />\r\n" +
                "</Track>";
            Track track = new()
            { 
                Uuid = "123456789",
                Category = GpsSignal.Normal,
                Route = "145",
                VehicleType = VehicleType.Bus,
                Point = new()
                {
                    Latitude = 55.753630,
                    Longitude = 55.753630,
                    AvgSpeed = 0,
                    Direction = 242,
                    Time = "10012009:142045",
                }
            };
            var resultValue = XmlSerializer.Serialize(track);
            if (resultValue != waitValue)
            {
                throw new Exception($"Error serialize value. Get `{resultValue}`, expected {resultValue}.");
            }
        }

        [Fact(DisplayName = "Xml формат Tracks")]
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

        [Fact(DisplayName = "Xml формат всего объекта")]
        public void TestXmlSerializerFullObject()
        {
            const string waitValue = 
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n"+
                "<tracks clid=\"123\">\r\n  "+
                    "<track uuid=\"123456789\" category=\"s\" route=\"145\" vehicle_type=\"tramway\">\r\n    " +
                        "<point latitude=\"55.75363\" longitude=\"55.75363\" avg_speed=\"0\" direction=\"242\" time=\"10012009:142045\" />\r\n  "+
                    "</track>\r\n"+
                "</tracks>";

            Track track = new()
            {
                Uuid = "123456789",
                Category = GpsSignal.Slow,
                Route = "145",
                VehicleType = VehicleType.Tramway,
                Point = new()
                {
                    Latitude = 55.753630,
                    Longitude = 55.753630,
                    AvgSpeed = 0,
                    Direction = 242,
                    Time = "10012009:142045",
                }
            };

            Tracks tracks = new("123", track);
            var resultValue = XmlSerializer.Serialize(tracks);
            if (resultValue != waitValue)
            {
                throw new Exception($"Error serialize value. Get `{resultValue}`, expected {resultValue}.");
            }
        }
    }
}
