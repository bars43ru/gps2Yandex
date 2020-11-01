namespace Gps2Yandex.Core.Entities
{
    public class AggregatedData
    {
        public Route Route { get; }
        public Schedule Schedule { get; }
        public Transport Transport { get; }
        public GpsPoint GpsPoint { get; }

        public AggregatedData(Route route, Schedule schedule, Transport transport, GpsPoint gpsPoint)
        {
            Route = route;
            Schedule = schedule;
            Transport = transport;
            GpsPoint = gpsPoint;
        }
    }
}
