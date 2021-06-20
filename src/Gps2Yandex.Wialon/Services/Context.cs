using Gps2Yandex.Wialon.Entities;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Gps2Yandex.Wialon.Services
{
    public class Context
    {
        ConcurrentDictionary<string, GpsPoint> gpsPoints { get; }
        public IEnumerable<GpsPoint> GpsPoints { get => gpsPoints.Values; }

        public Context()
        {
            gpsPoints = new ConcurrentDictionary<string, GpsPoint>();
        }

         public void Update(GpsPoint point)
        {
            if (!gpsPoints.TryRemove(point.MonitoringNumber, out var oldPoint))
            {
                oldPoint = point;
            }
            gpsPoints.TryAdd(point.MonitoringNumber, oldPoint.Time.CompareTo(point.Time) <= 0 ? point : oldPoint);
        }
    }
}
