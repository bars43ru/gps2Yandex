using System;

namespace Gps2Yandex.Dataset.Models
{
    /// <summary>
    /// Расписание
    /// </summary>
    internal class Schedule
    {
        /// <summary>
        /// Номер маршрута в учетной системе
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// Идентификатор машины в учетной системе (обычно гос. номер или гаражный номер на предприятии)
        /// </summary>
        public string Transport { get; set; }

        /// <summary>
        /// Начало работы на маршруте
        /// </summary>
        public DateTime Begin { get; set; }

        /// <summary>
        /// Окончание работы на маршруте
        /// </summary>
        public DateTime End { get; set; }
    }
}
