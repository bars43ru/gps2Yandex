namespace Gps2Yandex.Model.Entity
{
    /// <summary>
    /// Данные по маршруту (переводная таблица)
    /// </summary>
    public class Route
    {
        /// <summary>
        /// Номер маршрута в учетной системе (та которая присылает данные по расписанию)
        /// </summary>
        public string ExternalNumber { get; }

        /// <summary>
        /// Номер маршрута который отправляет интеграционный модуль в yandex
        /// </summary>
        public string YandexNumber { get; }

        public Route(string externalNumber, string yandexNumber)
        {
            ExternalNumber = externalNumber;
            YandexNumber = yandexNumber;
        }
    }
}
