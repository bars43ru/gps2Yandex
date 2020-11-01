using System.Collections.Generic;

using Gps2Yandex.Core.Entities;

namespace Gps2Yandex.Core.Interfaces
{
    public interface IDataset
    {
        IEnumerable<Route> Routes { get; }

        IEnumerable<Schedule> Schedules { get; }

        IEnumerable<Transport> Transports { get; }
    }
}
