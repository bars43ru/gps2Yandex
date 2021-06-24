using System;
using System.Linq;
using System.Collections.Generic;
using Gps2Yandex.References.Entities;

namespace Gps2Yandex.References.Services
{
    public class Context
    {
        public IEnumerable<Transport> Buses { get; private set; }
        public IEnumerable<Route> Routes { get; private set; }
        public IEnumerable<Schedule> Schedules { get; private set; }

        public Context()
        {
            Buses = Array.Empty<Transport>();
            Routes = Array.Empty<Route>();
            Schedules = Array.Empty<Schedule>();
        }

        public void Update(params Transport[] buses)
        {
            Buses = buses;
        }

        public void Update(params Route[] routes)
        {
            Routes = routes;
        }

        public void Update(params Schedule[] schedules)
        {
            Schedules = schedules;
        }

        public IEnumerable<Schedule> ActualSchedules(DateTime datetime, uint deviation)
        {
            var beginInterval = datetime.AddMinutes(deviation * -1);
            var endInterval = datetime.AddMinutes(deviation * +1);
            //var source = Schedules.Where(a => a.Begin >= beginInterval).OrderBy(a => a.Begin).ToList();
            //var result = Schedules.Where(a => a.Begin <= endInterval && a.End >= beginInterval).OrderBy(a => a.Begin).ToList();
            return Schedules.Where(a => a.Begin <= endInterval && a.End >= beginInterval).OrderBy(a => a.Begin);
        }
    }
}
