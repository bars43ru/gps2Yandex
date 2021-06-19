﻿using System;

namespace Gps2Yandex.Model.Entity
{
    public class GpsPoint
    {
        /// <summary>
        /// Идентификатор транспортного средства в системе мониторинга
        /// </summary>
        public string MonitoringNumber { get; }

        /// <summary>
        /// Дата и время сообщения
        /// </summary>
        public DateTime Time { get; }

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude { get; }
        
        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        /// Скорость
        /// </summary>
        public uint Speed { get; }

        /// <summary>
        /// Курс
        /// </summary>
        public uint Course { get; }

        /// <summary>
        /// Высота. Если отсутствует, значение null.
        /// </summary>
        //public int? Alt { get; set; }

        /// <summary>
        /// Количество спутников. Если отсутствует, значение null.
        /// </summary>
        //public int? Sats { get; set; }

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
