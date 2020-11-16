using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Gps2Yandex.Core.Entities;

using Minutes = System.UInt16;

namespace Gps2Yandex.Core.Services
{
    public class Context
    {
        ConcurrentDictionary<string, GpsPoint> gpsPoints { get; }
        public IEnumerable<Transport> Buses { get; private set; }
        public IEnumerable<Route> Routes { get; private set; }
        public IEnumerable<Schedule> Schedules { get; private set; }
        public IEnumerable<GpsPoint> GpsPoints { get => gpsPoints.Values; }

        public Context()
        {
            gpsPoints = new ConcurrentDictionary<string, GpsPoint>();
            Buses = Array.Empty<Transport>();
            Routes = Array.Empty<Route>();
            Schedules = Array.Empty<Schedule>();
        }

        public void Update(GpsPoint point)
        {
            if (!gpsPoints.TryRemove(point.MonitoringNumber, out var oldPoint))
            {
                oldPoint = point;
            }
            gpsPoints.TryAdd(point.MonitoringNumber, oldPoint.Time.CompareTo(point.Time) <= 0 ? point : oldPoint);
        }

        public IEnumerable<Schedule> ActualSchedules(DateTime datetime, Minutes deviation)
        {
            var beginInterval = datetime.AddMinutes(deviation * -1);
            var endInterval = datetime.AddMinutes(deviation * +1);
            //var source = Schedules.Where(a => a.Begin >= beginInterval).OrderBy(a => a.Begin).ToList();
            //var result = Schedules.Where(a => a.Begin <= endInterval && a.End >= beginInterval).OrderBy(a => a.Begin).ToList();
            return Schedules.Where(a => a.Begin <= endInterval && a.End >= beginInterval).OrderBy(a => a.Begin);
        }
    }
}
