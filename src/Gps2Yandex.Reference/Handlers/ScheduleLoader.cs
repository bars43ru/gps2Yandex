using System;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Gps2Yandex.References.Entities;

namespace Gps2Yandex.References.Handlers
{
    public static class ScheduleLoader
    {
        const string DateTimeFormat = @"dd/MM/yyyy'T'HH:mm:ss'Z'zzz";
        const string Pattern = @"(?<route>[^;]*);(?<transport>[^;]*);(?<begin>[^;]+);(?<end>[^;]+)";
        static Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(Pattern));

        /// <summary>
        /// Считывания данных о расписании
        /// </summary>
        /// <param name="file"><see cref="FileInfo"/> с сылкой на файл с данными о расписании</param>
        /// <returns>считанное расписание</returns>
        public static List<Schedule> Read(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            using var readStream = new StreamReader(fileStream);
            return Read(readStream);
        }

        /// <summary>
        /// Считывания данных о расписании
        /// </summary>
        /// <param name="reader"><see cref="StreamReader"/> с данными о расписании</param>
        /// <returns>считанное расписании</returns>
        public static List<Schedule> Read(StreamReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            List<Schedule> result = new(30);
            while (!reader.EndOfStream)
            {
                var record = reader.ReadLine();
                // пропускаем пустые строки и если в них только управляющие символы
                if (string.IsNullOrWhiteSpace(record))
                {
                    continue;
                }
                result.Add(Parse(record));
            };
            return result;
        }

        /// <summary>
        /// Парсим строку
        /// </summary>
        /// <param name="value">Строка с данными</param>
        /// <returns><seealso cref="Schedule"/>></returns>
        public static Schedule Parse(string value)
        {
            var match = Regex.Value.Match(value);
            if (match.Success)
            {
                return new Schedule(
                    route: match.Groups["route"].Value,
                    transport: match.Groups["transport"].Value,
                    begin: ToDateTime(match.Groups["begin"].Value),
                    end: ToDateTime(match.Groups["end"].Value)
                );
            }
            else
            {
                throw new FormatException($"The record `{value}` doesn't match the format `{Pattern}`.");
            }
        }

        public static DateTime ToDateTime(string value)
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
