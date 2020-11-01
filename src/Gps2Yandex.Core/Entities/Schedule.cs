using System;

namespace Gps2Yandex.Core.Entities
{
    /// <summary>
    /// Расписание
    /// </summary>
    public class Schedule
    {
        /// <summary>
        /// Номер маршрута в учетной системе
        /// </summary>
        public string Route { get; }

        /// <summary>
        /// Идентификатор машины в учетной системе (обычно гос. номер или гаражный номер на предприятии)
        /// </summary>
        public string Transport { get; }

        /// <summary>
        /// Начало работы на маршруте
        /// </summary>
        public DateTime Begin { get; }

        /// <summary>
        /// Окончание работы на маршруте
        /// </summary>
        public DateTime End { get; }

        public Schedule(string route, string transport, DateTime begin, DateTime end)
        {
            Route = route;
            Transport = transport;
            Begin = begin;
            End = end;
        }
    }
}
