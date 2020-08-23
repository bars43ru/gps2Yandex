namespace Gps2Yandex.Model.Entity
{
    /// <summary>
    /// Информация по транспортному средству (переводная таблица для соединения данных пришедние 
    /// из системы мониторинга с данными расписание пришешими из учетной системы)
    /// </summary>
    public class Transport
    {
        /// <summary>
        /// Идентификатор транспорта в системе мониторингда (система которая присылает телеметрию по транспортному средству)
        /// </summary>
        public string MonitoringNumber { get; }
        
        /// <summary>
        /// Идентификатор транспортного средства в учетной системе
        /// </summary>
        public string ExternalNumber { get; }

        /// <summary>
        /// Тип общественного транспортного средства в типах Yandex
        /// </summary>
        public string Type { get; }

        public Transport(string monitoringNumber, string externalNumber, string type)
        {
            MonitoringNumber = monitoringNumber;
            ExternalNumber = externalNumber;
            Type = type;
        }
    }
}
