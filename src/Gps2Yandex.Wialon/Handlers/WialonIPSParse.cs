using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Gps2Yandex.Model.Entity;

namespace Gps2Yandex.Wialon.Handlers
{
    public sealed class WialonIPSParse: IDisposable
    {
        private const string PatternL = @"#L#(?<uid>\w+);";
        private const string PatternD = @"#D#(?<date>\d+);(?<time>\d+);(?<lat1>\d+\.\d+);(?<lat2>\w+);(?<lon1>\d+\.\d+);(?<lon2>\w+);(?<speed>\d+);(?<course>\d+);(?<alt>\d+\.\d+);(?<sats>\d+)";

        private readonly StreamReader reader;

        public WialonIPSParse(Stream stream)
        {
            reader = new StreamReader(stream);
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        /// <summary>
        /// Чтения заголовка
        /// </summary>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Считанная строка</returns>
        private async Task<string> ReadHeader(CancellationToken token = default)
        {
            var readLine = reader.ReadLineAsync();
            Task.WaitAny(new[] { readLine }, token);
            token.ThrowIfCancellationRequested();
            return await readLine ?? "";
        }

        /// <summary>
        /// Чтение ретрансляционных данных по сообщениям
        /// </summary>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns>Считанная строка</returns>
        private async IAsyncEnumerable<string> ReadBody([EnumeratorCancellation] CancellationToken token = default)
        {
            while (!reader.EndOfStream)
            {
                var readLine = reader.ReadLineAsync();
                Task.WaitAny(new[] { readLine }, token);
                token.ThrowIfCancellationRequested();
                yield return await readLine ?? "";
            }
        }

        /// <summary>
        /// Получение ретрансляционных сообщений по мере поступления
        /// </summary>
        /// <param name="token"><see cref="CancellationToken"/></param>
        /// <returns><see cref="GpsPoint"/></returns>
        public async IAsyncEnumerable<GpsPoint> Messages([EnumeratorCancellation] CancellationToken token = default)
        {
            var header = await ReadHeader(token);
            if (string.IsNullOrEmpty(header))
            {
                throw new FormatException($"The device ID is missing in the first line of the stream.");
            }

            Regex regex = new(PatternL);
            Match match = regex.Match(header);
            if (!match.Success)
            {
                throw new FormatException($"Header `{header}` in received data packet, doesn't match the format `{PatternL}`.");
            }
            var uid = match.Groups["uid"].Value;

            regex = new Regex(PatternD);
            await foreach(var message in ReadBody(token).ConfigureAwait(false))
            {
                match = regex.Match(message);
                if (!match.Success)
                {
                    throw new FormatException($"Message `{message}` in received data packet, doesn't match the format `{PatternD}`.");
                }

                var point = new GpsPoint(
                    monitoringNumber: uid,
                    time: Convert($"{match.Groups["date"].Value}{match.Groups["time"].Value}"),
                    latitude: Wgs84(double.Parse(match.Groups["lat1"].Value, CultureInfo.InvariantCulture)),
                    longitude: Wgs84(double.Parse(match.Groups["lon1"].Value, CultureInfo.InvariantCulture)),
                    speed: int.Parse(match.Groups["speed"].Value),
                    course: int.Parse(match.Groups["course"].Value)
                 );

                // Причина появления этого условия см. https://github.com/bars43ru/gps2Yandex/issues/11
                if ((int)point.Latitude == 90 && (int)point.Longitude == 0)
                {
                    continue;
                }

                yield return point;
            }
        }

        /// <summary>
        /// Преобразование даты и времени из строки формата 'ddMMyyHHmmss' в DateTime
        /// </summary>
        /// <param name="value">Дата и время в строковом представлении</param>
        /// <returns><see cref="DateTime"/></returns>
        private static DateTime Convert(string value)
        {
            var formats = new[] { "ddMMyyHHmmss" };
            if (!DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var change))
            {
                throw new ArgumentException($"value is not in proper `{formats[0]}` format.");
            }
            return change;
        }

        /// <summary>
        /// Конвертирование системы координат
        /// </summary>
        private static double Wgs84(double value)
        {
            var degrees = Math.Truncate(value / 100);
            //var minutes = Math.Truncate(dLat) - degrees;
            //var seconds = (dLat - degrees) * 10000;
            return Math.Round(degrees + (value - degrees * 100) / 60, 6);
        }
    }
}
