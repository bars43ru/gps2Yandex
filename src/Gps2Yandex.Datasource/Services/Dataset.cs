using System;
using System.Collections.Generic;

using Gps2Yandex.Core.Entities;
using Gps2Yandex.Core.Interfaces;

namespace Gps2Yandex.Datasource.Services
{
    internal class Dataset : IDataset
    {
        #region implementation IDataset
        IEnumerable<Route> IDataset.Routes => Routes;
        IEnumerable<Schedule> IDataset.Schedules => Schedules;
        IEnumerable<Transport> IDataset.Transports => Transports;
        #endregion

        public Route[] Routes { get; private set; }
        public Schedule[] Schedules { get; private set; }
        public Transport[] Transports { get; private set; }


        public Dataset()
        {
            Routes = Array.Empty<Route>();
            Schedules = Array.Empty<Schedule>();
            Transports = Array.Empty<Transport>();
        }

        public void Update(params Transport[] transports)
        {
            Transports = transports;
        }

        public void Update(params Route[] routes)
        {
            Routes = routes;
        }

        public void Update(params Schedule[] schedules)
        {
            Schedules = schedules;
        }
    }
}
