using Gps2Yandex.Wialon.Entities;
using Gps2Yandex.Wialon.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace Gps2Yandex.Wialon.Test
{
    public class UnitTestWialonIPSParse
    {
        [Fact(DisplayName = "Получение координат и их последовательность")]
        public async void TestCheckInformation()
        {
            const string testData =
@"#L#353173067939817;NA
#D#060521;081606;5844.6826;N;05010.7126;E;8;131;113.000000;15;7.000000;3;NA;NA;;SOS:1:1,avl_driver:3:,Odom:1:851171,Speed:1:9
#D#060521;081608;5844.6802;N;05010.7173;E;12;134;113.000000;15;7.000000;3;NA;NA;;SOS:1:1,avl_driver:3:,Odom:1:851171
#D#060521;081609;5844.6792;N;05010.7199;E;12;127;113.000000;15;7.000000;3;NA;NA;;SOS:1:1,avl_driver:3:,Odom:1:851171,Speed:1:10
#D#060521;081632;5844.5945;N;05010.7628;E;48;192;112.000000;14;7.000000;3;NA;NA;;SOS:1:1,avl_driver:3:,Odom:1:851171,Speed:1:45,Fuel_L_0:1:81,Fuel_T_0:1:9
";

            var msg = new List<GpsPoint>() {
                new ("353173067939817", new DateTime(2021,05, 06, 08, 16, 06, DateTimeKind.Utc), 58.74471, 50.178543, 8, 131),
                new ("353173067939817", new DateTime(2021,05, 06, 08, 16, 08, DateTimeKind.Utc), 58.74467, 50.178622, 12, 134),
                new ("353173067939817", new DateTime(2021,05, 06, 08, 16, 09, DateTimeKind.Utc), 58.744653, 50.178665, 12, 127),
                new ("353173067939817", new DateTime(2021,05, 06, 08, 16, 32, DateTimeKind.Utc), 58.743242, 50.17938, 48, 192)
            };

            byte[] byteArray = Encoding.ASCII.GetBytes(testData);
            using var ms = new MemoryStream(byteArray);

            using var parse = new WialonIPSParse(ms);
            
            await foreach(var p in parse.Messages(CancellationToken.None))
            {
                var waitCurrent = msg[0];
                if (waitCurrent.MonitoringNumber != p.MonitoringNumber ||
                    waitCurrent.Time != p.Time.ToUniversalTime() ||
                    waitCurrent.Latitude != p.Latitude ||
                    waitCurrent.Longitude != p.Longitude ||
                    waitCurrent.Speed != p.Speed ||
                    waitCurrent.Course != p.Course)
                {
                    throw new Exception($"Error parse value. Get {p}, expected {msg[0]}.");
                }
                msg.Remove(waitCurrent);
            }
        }

        [Fact(DisplayName = "Игнорирование координат вида 9000.0000,N,00000.0000,E")]
        public async void TestCheckInformationSkip()
        {
            const string testData =
@"#L#353173067939817;NA
#D#060521;081606;9000.0000;N;00000.0000;E;8;131;113.000000;15;7.000000;3;NA;NA;;SOS:1:1,avl_driver:3:,Odom:1:851171,Speed:1:9
";

            byte[] byteArray = Encoding.ASCII.GetBytes(testData);
            using var ms = new MemoryStream(byteArray);

            using var parse = new WialonIPSParse(ms);

            await foreach (var p in parse.Messages(CancellationToken.None))
            {
                throw new Exception($"No coordinates were expected.");
            }
        }

        [Fact(DisplayName = "Wialon id содержит символы")]
        public async void TestCheckCharId()
        {
            const string testData =
 @"#L#353f73A6939817;NA
#D#060521;081606;5844.6826;N;05010.7126;E;8;131;113.000000;15;7.000000;3;NA;NA;;SOS:1:1,avl_driver:3:,Odom:1:851171,Speed:1:9
";

            var msg = new List<GpsPoint>() {
                new ("353f73A6939817", new DateTime(2021,05, 06, 08, 16, 06, DateTimeKind.Utc), 58.74471, 50.178543, 8, 131),
            };

            byte[] byteArray = Encoding.ASCII.GetBytes(testData);
            using var ms = new MemoryStream(byteArray);

            using var parse = new WialonIPSParse(ms);

            await foreach (var p in parse.Messages(CancellationToken.None))
            {
                var waitCurrent = msg[0];
                if (waitCurrent.MonitoringNumber != p.MonitoringNumber ||
                    waitCurrent.Time != p.Time.ToUniversalTime() ||
                    waitCurrent.Latitude != p.Latitude ||
                    waitCurrent.Longitude != p.Longitude ||
                    waitCurrent.Speed != p.Speed ||
                    waitCurrent.Course != p.Course)
                {
                    throw new Exception($"Error parse value. Get {p}, expected {msg[0]}.");
                }
                msg.Remove(waitCurrent);
            }
        }

        [Fact(DisplayName = "Ошибка формата заголовка")]
        public async void TestHeaderFormatError()
        {
            const string testData =
 @"#L#353fн73A6939817;NA
#D#060521;081606;5844.6826;N;05010.7126;E;8;131;113.000000;15;7.000000;3;NA;NA;;SOS:1:1,avl_driver:3:,Odom:1:851171,Speed:1:9
";

            byte[] byteArray = Encoding.ASCII.GetBytes(testData);
            using var ms = new MemoryStream(byteArray);

            using var parse = new WialonIPSParse(ms);
            try
            {
                await foreach (var p in parse.Messages(CancellationToken.None))
                {
                    throw new Exception($"A message was received, a format error was expected.");
                }
            }
            catch(FormatException)
            {
            }
        }

        [Fact(DisplayName = "В заголовке нет нечего")]
        public async void TestHeaderFormatError2()
        {
            const string testData = @"";
            byte[] byteArray = Encoding.ASCII.GetBytes(testData);
            using var ms = new MemoryStream(byteArray);

            using var parse = new WialonIPSParse(ms);
            try
            {
                await foreach (var p in parse.Messages(CancellationToken.None))
                {
                    throw new Exception($"A message was received, a format error was expected.");
                }
            }
            catch (FormatException)
            {
            }
        }

        [Fact(DisplayName = "Ошибка формата тела сообщения")]
        public async void TesMessageFormatError()
        {
            const string testData =
 @"#L#353f73A6939817;NA
#D#060521;081606;.6826;N;05010.7126;E;8;131;113.000000;15;7.000000;3;NA;NA;;SOS:1:1,avl_driver:3:,Odom:1:851171,Speed:1:9
";

            byte[] byteArray = Encoding.ASCII.GetBytes(testData);
            using var ms = new MemoryStream(byteArray);

            using var parse = new WialonIPSParse(ms);
            try
            {
                await foreach (var p in parse.Messages(CancellationToken.None))
                {
                    throw new Exception($"A message was received, a format error was expected.");
                }
            }
            catch (FormatException)
            {
            }
        }
    }
}
