using System.Linq;

using Gps2Yandex.Core.Entities;
using Gps2Yandex.Core.Interfaces;

namespace Gps2Yandex.Core.Handlers
{
    internal static class DatasetExtensions
    {
        public static AggregatedData LinkData(this IDataset dataset, GpsPoint gpsPoint)
        {
            var transport = dataset.Transports.SingleOrDefault(t => t.MonitoringNumber == gpsPoint.MonitoringNumber);
            var schedule = dataset.Schedules
                .Where(s => s.Transport == transport.ExternalNumber)
                .Where(s => s.Begin <= gpsPoint.Time && s.End >= gpsPoint.Time)
                .OrderBy(o => o.Begin)
                .FirstOrDefault();
            var route = schedule != null ? dataset.Routes.FirstOrDefault(r => r.ExternalNumber == schedule.Route) : null;
            return new AggregatedData(route, schedule, transport, gpsPoint);
        }
    }
}
