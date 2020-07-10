using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using Gps2Yandex.Dataset.Models;

namespace Gps2Yandex.Dataset.Services
{
    internal class RouteLoader
    {
        const string Pattern = @"(?<ExternalNumber>[^;]*);(?<YandexNumber>[^;]*)";
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
            Context.Update(ReadAll(readStream));
        }

        private IEnumerable<Route> ReadAll(StreamReader reader)
        {
            List<Route> result = new List<Route>();
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
                    result.Add(new Route(match.Groups["ExternalNumber"].Value, match.Groups["YandexNumber"].Value));
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
