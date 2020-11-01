using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Gps2Yandex.Core.Entities;
using Gps2Yandex.Datasource.Services;

namespace Gps2Yandex.Datasource.Handlers
{
    internal class TransportLoader
    {
        const string Pattern = @"(?<uid>[^;]*);(?<state>[^;]*);(?<type>[^;]*)";
        Lazy<Regex> Regex { get; } = new Lazy<Regex>(() => new Regex(Pattern));
        Dataset Dataset { get; }

        public TransportLoader(Dataset dataset)
        {
            Dataset = dataset ?? throw new ArgumentNullException();
        }

        public void LoadFrom(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            using var readStream = new StreamReader(fileStream);
            Dataset.Update(ReadAll(readStream).ToArray());
        }

        private IEnumerable<Transport> ReadAll(StreamReader reader)
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
        /// <returns><seealso cref="Transport"/>></returns>
        private Transport Parse(string value)
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
