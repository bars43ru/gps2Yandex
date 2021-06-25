using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Gps2Yandex.References.Entities;

namespace Gps2Yandex.References.Handlers
{
    public static class RouteLoader
    {
        const string Pattern = @"(?<external>[^;]*);(?<yandex>[^;]*)";
        static Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(Pattern));

        /// <summary>
        /// Считывания данных о маршрутах
        /// </summary>
        /// <param name="file"><see cref="FileInfo"/> с сылкой на файл с данными о маршрутах</param>
        /// <returns>считанные маршруты</returns>
        public static List<Route> Read(FileInfo file)
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
        /// Считывания данных о маршрутах
        /// </summary>
        /// <param name="reader"><see cref="StreamReader"/> с данными о маршрутах</param>
        /// <returns>считанные маршруты</returns>
        public static List<Route> Read(StreamReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            List<Route> result = new(30);
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
        /// Парсинг строка
        /// </summary>
        /// <param name="value">Строка с данными</param>
        /// <returns><seealso cref="Route"/>></returns>
        public static Route Parse(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var match = Regex.Value.Match(value);
            if (match.Success)
            {
                return new Route(
                    externalNumber: match.Groups["external"].Value, 
                    yandexNumber: match.Groups["yandex"].Value);
            }
            else
            {
                throw new FormatException($"The record `{value}` doesn't match the format `{Pattern}`.");
            }
        }
    }
}
