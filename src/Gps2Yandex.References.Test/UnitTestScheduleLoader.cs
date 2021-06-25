using System;
using System.IO;
using System.Text;
using Xunit;
using Gps2Yandex.References.Handlers;

namespace Gps2Yandex.References.Test
{
    public class UnitTestScheduleLoader
    {
        [Fact(DisplayName = "Парсинг корректной строки с данными расписания")]
        public void TestParseSuccess()
        {
            const string testData = "928;Y009OT;02/06/2020T04:55:00Z+03:00;02/06/2020T06:00:00Z+03:00";
            var schedule = ScheduleLoader.Parse(testData);
            if (schedule.Route != "928" || schedule.Transport != "Y009OT"
                || schedule.Begin != new DateTime(2020, 06, 02, 01, 55, 00, DateTimeKind.Utc).ToLocalTime()
                || schedule.End != new DateTime(2020, 06, 02, 3, 00, 00, DateTimeKind.Utc).ToLocalTime())
            {
                throw new Exception($"Parse schedule.");
            }
        }

        [Fact(DisplayName = "Парсинг некорректной строки с данными расписания")]
        public void TestParseFailed()
        {
            const string testData = "928;Y009OT;02/06/2020T04:65:00Z+03:00;02/06/2020T06:00:00Z+03:00";
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
                _ = ScheduleLoader.Parse(testData);
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
928;Y009OT;02/06/2020T04:55:00Z+03:00;02/06/2020T06:00:00Z+03:00
928;A590OM;02/06/2020T15:50:00Z+03:00;02/06/2020T16:55:00Z+03:00

102;Y009OT;02/06/2020T06:30:00Z+03:00;02/06/2020T13:30:00Z+03:00

102;A590OM;02/06/2020T05:25:00Z+03:00;02/06/2020T12:30:00Z+03:00

";
            byte[] byteArray = Encoding.ASCII.GetBytes(testData);
            using var ms = new MemoryStream(byteArray);
            using var sr = new StreamReader(ms);
            var schedules = ScheduleLoader.Read(sr);
            if (schedules.Count != 4)
            {
                throw new Exception($"4 routes were expected, {schedules.Count} were considered.");
            }
        }
    }
}
