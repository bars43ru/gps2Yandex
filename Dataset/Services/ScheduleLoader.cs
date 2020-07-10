using System;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Gps2Yandex.Dataset.Models;

namespace Gps2Yandex.Dataset.Services
{
    internal class ScheduleLoader
    {
        const string DateTimeFormat = @"dd/MM/yyyy'T'HH:mm:ss'Z'zzz";
        const string Pattern = @"(?<Route>[^;]*);(?<Transport>[^;]*);(?<Begin>[^;]+);(?<End>[^;]+)";
        Context Context { get; }

        public ScheduleLoader(Context context)
        {
            Context = context ?? throw new ArgumentNullException();
        }

        public void LoadFrom(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            using var readStream = new StreamReader(fileStream);
            Context.Update(ReadAll(readStream));
        }

        private IEnumerable<Schedule> ReadAll(StreamReader reader)
        {
            var result = new List<Schedule>();
            Regex regex = new Regex(Pattern);
            string record;
            while (!reader.EndOfStream)
            {
                record = reader.ReadLine();
                // пропускаем пустые строки и если в них только управляющие символы
                if (string.IsNullOrWhiteSpace(record?.Trim()))
                {
                    continue;
                }
                // получаем данные из считанной строки данные
                var match = regex.Match(record);
                if (match.Success)
                {
                    result.Add(new Schedule()
                    {
                        Route = match.Groups["Route"].Value,
                        Transport = match.Groups["Transport"].Value,
                        Begin = ToDateTime(match.Groups["Begin"].Value),
                        End = ToDateTime(match.Groups["End"].Value),
                    });
                }
                else
                {
                    throw new FormatException($"The record `{record}` doesn't match the format `{Pattern}`.");
                }
            };
            return result;
        }

        private DateTime ToDateTime(string value)
        {
            var formats = new string[] { DateTimeFormat };
            if (!DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime _change))
            {
                throw new ArgumentException($"Value `{value}` is not in proper `{DateTimeFormat}` format.");
            }
            return _change;
        }
    }
}
