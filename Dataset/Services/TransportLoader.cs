using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Gps2Yandex.Dataset.Models;

namespace Gps2Yandex.Dataset.Services
{
    internal class TransportLoader
    {
        const string Pattern = @"(?<uid>[^;]*);(?<state>[^;]*);(?<type>[^;]*)";
        Context Context { get; }
        public TransportLoader(Context context)
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

        private IEnumerable<Transport> ReadAll(StreamReader reader)
        {
            List<Transport> result = new List<Transport>();
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
                    result.Add(new Transport(
                        monitoringNumber: match.Groups["uid"].Value,
                        externalNumber: match.Groups["state"].Value,
                        type: match.Groups["type"].Value));
                }
                else
                {
                    throw new FormatException($"The record `{record}` doesn't match the format `{Pattern}`.");
                }
            };
            return result;
        }
    }
}
