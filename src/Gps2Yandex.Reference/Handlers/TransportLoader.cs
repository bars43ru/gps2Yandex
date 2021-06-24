using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Gps2Yandex.References.Entities;

namespace Gps2Yandex.References.Handlers
{
    public static class TransportLoader
    {
        const string Pattern = @"(?<uid>[^;]*);(?<state>[^;]*);(?<type>[^;]*)";
        static Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(Pattern));

        /// <summary>
        /// Считывания данных о транспорте
        /// </summary>
        /// <param name="file"><see cref="FileInfo"/> с сылкой на файл с данными о транспорте</param>
        /// <returns>считанные маршруты</returns>
        public static List<Transport> Read(FileInfo file)
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
        /// Считывания данных о транспорте
        /// </summary>
        /// <param name="reader"><see cref="StreamReader"/> с данными о транспорте</param>
        /// <returns>считанный транспорт</returns>
        public static List<Transport> Read(StreamReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            List<Transport> result = new(30);
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
        /// <returns><seealso cref="Transport"/>></returns>
        public static Transport Parse(string value)
        {
            var match = Regex.Value.Match(value);
            if (match.Success)
            {
                return new Transport(
                    monitoringNumber: match.Groups["uid"].Value,
                    externalNumber: match.Groups["state"].Value,
                    type: match.Groups["type"].Value);
            }
            else
            {
                throw new FormatException($"The record `{value}` doesn't match the format `{Pattern}`.");
            }
        }
    }
}
