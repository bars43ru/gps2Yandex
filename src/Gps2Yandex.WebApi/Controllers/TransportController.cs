using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Gps2Yandex.Core.Interfaces;
using Gps2Yandex.Core.Entities;

namespace Gps2Yandex.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TransportController : ControllerBase
    {
        private ILogger Logger { get; }
        private IDataset Dataset { get; }

        public TransportController(
            ILogger<RouteController> logger,
            IDataset dataset)
        {
            Logger = logger;
            Dataset = dataset;
        }

        public IEnumerable<Transport> List()
        {
            Logger.LogInformation(nameof(List));
            return Dataset.Transports;
        }
    }
}
