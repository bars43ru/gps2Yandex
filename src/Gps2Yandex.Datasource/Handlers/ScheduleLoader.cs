using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Gps2Yandex.Core.Entities;
using Gps2Yandex.Datasource.Services;

namespace Gps2Yandex.Datasource.Handlers
{
    internal class ScheduleLoader
    {
        private const string DateTimeFormat = @"dd/MM/yyyy'T'HH:mm:ss'Z'zzz";
        private const string Pattern = @"(?<route>[^;]*);(?<transport>[^;]*);(?<begin>[^;]+);(?<end>[^;]+)";
        private Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(Pattern));
        private Dataset Dataset { get; }

        public ScheduleLoader(Dataset dataset)
        {
            Dataset = dataset ?? throw new ArgumentNullException(nameof(dataset));
        }

        public void LoadFrom(FileInfo file)
        {
            _ = file ?? throw new ArgumentNullException(nameof(file));
            using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            using var readStream = new StreamReader(fileStream);
            Dataset.Update(ReadAll(readStream).ToArray());
        }

        private IEnumerable<Schedule> ReadAll(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                var record = reader.ReadLine();
                // пропускаем пустые строки и если в них только управляющие символы
                if (!string.IsNullOrWhiteSpace(record)) yield return Parse(record);
            }
        }

        /// <summary>
        /// Парсим строку
        /// </summary>
        /// <param name="value">Строка с данными</param>
        /// <returns><seealso cref="Schedule"/>></returns>
        private Schedule Parse(string value)
        {
            var match = Regex.Value.Match(value);
            return match.Success
                ? new Schedule(
                    route: match.Groups["route"].Value,
                    transport: match.Groups["transport"].Value,
                    begin: ToDateTime(match.Groups["begin"].Value),
                    end: ToDateTime(match.Groups["end"].Value)
                )
                : throw new FormatException($"The record `{value}` doesn't match the format `{Pattern}`.");
        }


        private static DateTime ToDateTime(string value)
        {
            var formats = new[] { DateTimeFormat };
            return !DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal,
                out var change)
                ? throw new ArgumentException($"Value `{value}` is not in proper `{DateTimeFormat}` format.")
                : change;
        }
    }
}
