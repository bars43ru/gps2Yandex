using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;

using Gps2Yandex.Dataset.Models;

namespace Gps2Yandex.Wialon.Extensions
{
    internal static class StreamReaderExtensions
    {
        private static string PatternL { get; } = @"#L#(?<uid>\d+)";
        private static string PatternD { get; } = @"#D#(?<date>\d+);(?<time>\d+);(?<lat1>\d+\.\d+);(?<lat2>\w+);(?<lon1>\d+\.\d+);(?<lon2>\w+);(?<speed>\d+);(?<course>\d+);(?<alt>\d+\.\d+);(?<sats>\d+)";
        public static IEnumerable<GpsPoint> ReadAll(this NetworkStream streamReader)
        {
            if (streamReader == null)
            {
                throw new ArgumentNullException(nameof(streamReader));
            }
            using var source = new StreamReader(streamReader);

            string header = source.ReadLine();

            Regex regex = new Regex(PatternL);
            Match match = regex.Match(header);
            if (!match.Success)
            {
                yield break;
            }
            string uid = match.Groups["uid"].Value;

            regex = new Regex(PatternD);

            string value;
            while (!source.EndOfStream)
            {
                value = source.ReadLine() ?? "";
                match = regex.Match(value);
                if (!match.Success)
                {
                    continue;
                }

                yield return new GpsPoint(
                    monitoringNumber: uid,
                    time: $"{match.Groups["date"].Value}{match.Groups["time"].Value}".ToDateTime(),
                    latitude: double.Parse(match.Groups["lat1"].Value, CultureInfo.InvariantCulture).ToWGS84(),
                    longitude: double.Parse(match.Groups["lon1"].Value, CultureInfo.InvariantCulture).ToWGS84(),
                    speed: int.Parse(match.Groups["speed"].Value),
                    course: int.Parse(match.Groups["course"].Value)
                    );
            }
        }

        private static DateTime ToDateTime(this string value)
        {
            var formats = new string[] { "ddMMyyHHmmss" };
            if (!DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime _change))
            {
                throw new ArgumentException($"value is not in proper `{formats[0]}` format.");
            }
            return _change;
        }

        private static double ToWGS84(this double value)
        {
            var degrees = Math.Truncate(value / 100);
            //var minutes = Math.Truncate(dLat) - degrees;
            //var seconds = (dLat - degrees) * 10000;
            return Math.Round(degrees + (value - degrees * 100) / 60, 6);
        }
    }
}
