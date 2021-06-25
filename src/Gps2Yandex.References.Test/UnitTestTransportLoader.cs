using System;
using System.IO;
using System.Text;
using Xunit;
using Gps2Yandex.References.Handlers;

namespace Gps2Yandex.References.Test
{
    public class UnitTestTransportLoader
    {
        [Fact(DisplayName = "Парсинг корректной строки с данными транспорта")]
        public void TestParseSuccess()
        {
            const string testData = "353173067906170;E111OK;bus";
            var transport = TransportLoader.Parse(testData);
            if (transport.ExternalNumber != "E111OK" || transport.MonitoringNumber != "353173067906170"
                || transport.Type != "bus")
            {
                throw new Exception($"Parse transpor.");
            }
        }

        [Fact(DisplayName = "Парсинг некорректной строки с данными транспорта")]
        public void TestParseFailed()
        {
            const string testData = "53173067906170E111OK;bus";
            try
            {
                _ = ScheduleLoader.Parse(testData);
            }
            catch (FormatException)
            {
                return;
            }
            throw new Exception($"An exception was expected `FormatException`.");
        }

        [Fact(DisplayName = "Парсинг пустой строки")]
        public void TestParseEmptyStringFailed()
        {
            const string testData = "";
            try
            {
                _ = TransportLoader.Parse(testData);
            }
            catch (FormatException)
            {
                return;
            }
            throw new Exception($"An exception was expected `FormatException`.");
        }


        [Fact(DisplayName = "Парсинг stream с пропуском пустых строк")]
        public void TestParseStreamSkipeEmptySuccess()
        {
            const string testData =
 @"

353173067906170;E111OK;bus
353173068562600;E222OK;bus
";
            byte[] byteArray = Encoding.ASCII.GetBytes(testData);
            using var ms = new MemoryStream(byteArray);
            using var sr = new StreamReader(ms);
            var transports = TransportLoader.Read(sr);
            if (transports.Count != 2)
            {
                throw new Exception($"2 routes were expected, {transports.Count} were considered.");
            }
        }
    }
}
