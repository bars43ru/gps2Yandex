using System;
using System.IO;
using System.Text;
using Xunit;
using Gps2Yandex.References.Handlers;

namespace Gps2Yandex.References.Test
{
    public class UnitTestRouteLoader
    {
        [Fact(DisplayName = "Парсинг корректной строки с данными о маршруте")]
        public void TestParseSuccess()
        {
            const string testData = "103э;103Э";
            var route = RouteLoader.Parse(testData);
            if (route.ExternalNumber != "103э" || route.YandexNumber != "103Э")
            {
                throw new Exception($"Parse route.");
            }
        }

        [Fact(DisplayName = "Парсинг некорректной строки с данными о маршруте")]
        public void TestParseFailed()
        {
            const string testData = "103э";
            try
            {
                _ = RouteLoader.Parse(testData);
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
                _ = RouteLoader.Parse(testData);
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
 @"102;103
   

102;104
";
            byte[] byteArray = Encoding.ASCII.GetBytes(testData);
            using var ms = new MemoryStream(byteArray);
            using var sr = new StreamReader(ms);
            var routes = RouteLoader.Read(sr);
            if (routes.Count != 2)
            {
                throw new Exception($"2 routes were expected, {routes.Count} were considered.");
            }
        }
    }
}
