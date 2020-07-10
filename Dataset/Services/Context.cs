using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Gps2Yandex.Dataset.Models;

using Minutes = System.UInt16;

namespace Gps2Yandex.Dataset.Services
{
    class Context
    {
        ConcurrentDictionary<string, GpsPoint> gpsPoints { get; }
        public IEnumerable<Transport> Buses { get; private set; }
        public IEnumerable<Route> Routes { get; private set; } 
        public IEnumerable<Schedule> Schedules { get; private set; }
        public IEnumerable<GpsPoint> GpsPoints { get => gpsPoints.Values; }

        public Context()
        {
            gpsPoints = new ConcurrentDictionary<string, GpsPoint>();
            Buses = new Transport[] { };
            Routes = new Route[] { };
            Schedules = new Schedule[] { };
        }

        public void Update(IEnumerable<Transport> buses)
        {
            Buses = buses;
        }

        public void Update(IEnumerable<Route> routes)
        {
            Routes = routes;
        }

        public void Update(IEnumerable<Schedule> schedules)
        {
            Schedules = schedules;
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
