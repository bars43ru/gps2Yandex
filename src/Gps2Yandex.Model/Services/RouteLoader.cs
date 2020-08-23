using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Gps2Yandex.Model.Entity;

namespace Gps2Yandex.Model.Services
{
    internal class RouteLoader
    {
        const string Pattern = @"(?<external>[^;]*);(?<yandex>[^;]*)";
        Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(Pattern));
        Context Context { get; }

        public RouteLoader(Context context)
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
            Context.Update(ReadAll(readStream).ToArray());
        }

        /// <summary>
        /// Считывания данных о маршрутах
        /// </summary>
        /// <param name="reader"><seealso cref="StreamReader"/> на файл></param>
        /// <returns>Перечень маршрутов</returns>
        private IEnumerable<Route> ReadAll(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                var record = reader.ReadLine();
                // пропускаем пустые строки и если в них только управляющие символы
                if (string.IsNullOrWhiteSpace(record))
                {
                    continue;
                }
                yield return Parse(record);
            };
        }

        /// <summary>
        /// Парсим строку
        /// </summary>
        /// <param name="value">Строка с данными</param>
        /// <returns><seealso cref="Route"/>></returns>
        private Route Parse(string value)
        {
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
