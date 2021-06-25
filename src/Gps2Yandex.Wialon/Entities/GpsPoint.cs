using System;

namespace Gps2Yandex.Wialon.Entities
{
    public struct GpsPoint
    {
        /// <summary>
        /// Идентификатор транспортного средства в системе мониторинга
        /// </summary>
        public string MonitoringNumber { get; init; }

        /// <summary>
        /// Дата и время сообщения
        /// </summary>
        public DateTime Time { get; init; }

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude { get; init; }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude { get; init; }

        /// <summary>
        /// Скорость
        /// </summary>
        public uint Speed { get; init; }

        /// <summary>
        /// Курс
        /// </summary>
        public uint Course { get; init; }

        /// <summary>
        /// Высота. Если отсутствует, значение null.
        /// </summary>
        //public int? Alt { get; init; }

        /// <summary>
        /// Количество спутников. Если отсутствует, значение null.
        /// </summary>
        //public int? Sats { get; init; }

        public GpsPoint(string monitoringNumber, DateTime time, double latitude, double longitude, uint speed, uint course)
        {
            MonitoringNumber = monitoringNumber;
            Time = time;
            Latitude = latitude;
            Longitude = longitude;
            Speed = speed;
            Course = course;
        }
    }
}
