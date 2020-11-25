using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;

using Gps2Yandex.Core.Entities;

namespace Gps2Yandex.Wialon.Extensions
{
    internal static class StreamReaderExtensions
    {
        private static string PatternL { get; } = @"#L#(?<uid>\w+)";
        private static string PatternD { get; } = @"#D#(?<date>\d+);(?<time>\d+);(?<lat1>\d+\.\d+);(?<lat2>\w+);(?<lon1>\d+\.\d+);(?<lon2>\w+);(?<speed>\d+);(?<course>\d+);(?<alt>\d+\.\d+);(?<sats>\d+)";
        public static IEnumerable<GpsPoint> ReadAll(this NetworkStream streamReader)
        {
            if (streamReader == null)
            {
                throw new ArgumentNullException(nameof(streamReader));
            }
            using var source = new StreamReader(streamReader);

            var header = source.ReadLine();
            if (string.IsNullOrEmpty(header))
            {
                throw new FormatException($"The device ID is missing in the first line of the stream.");
            }

            Regex regex = new Regex(PatternL);
            Match match = regex.Match(header);
            if (!match.Success)
            {
                yield break;
            }
            var uid = match.Groups["uid"].Value;

            regex = new Regex(PatternD);

            while (!source.EndOfStream)
            {
                var value = source.ReadLine() ?? "";
                match = regex.Match(value);
                if (!match.Success)
                {
                    continue;
                }

                var point = new GpsPoint(
                    monitoringNumber: uid,
                    time: $"{match.Groups["date"].Value}{match.Groups["time"].Value}".ToDateTime(),
                    latitude: double.Parse(match.Groups["lat1"].Value, CultureInfo.InvariantCulture).ToWgs84(),
                    longitude: double.Parse(match.Groups["lon1"].Value, CultureInfo.InvariantCulture).ToWgs84(),
                    speed: int.Parse(match.Groups["speed"].Value),
                    course: int.Parse(match.Groups["course"].Value)
                 );

                // Причина появления этого условия см. https://github.com/bars43ru/gps2Yandex/issues/11
                if ((int)point.Latitude == 9000 && (int)point.Longitude == 0)
                {
                    continue;
                }

                yield return point;
            }
        }

        private static DateTime ToDateTime(this string value)
        {
            var formats = new[] { "ddMMyyHHmmss" };
            if (!DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var change))
            {
                throw new ArgumentException($"value is not in proper `{formats[0]}` format.");
            }
            return change;
        }

        private static double ToWgs84(this double value)
        {
            var degrees = Math.Truncate(value / 100);
            //var minutes = Math.Truncate(dLat) - degrees;
            //var seconds = (dLat - degrees) * 10000;
            return Math.Round(degrees + (value - degrees * 100) / 60, 6);
        }
    }
}
