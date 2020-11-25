using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

using Gps2Yandex.Core.Interfaces;
using Gps2Yandex.Core.Entities;

namespace Gps2Yandex.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RouteController : ControllerBase
    {
        private ILogger Logger { get; }
        private IDataset Dataset { get; }

        public RouteController(
            ILogger<RouteController> logger,
            IDataset dataset)
        {
            Logger = logger;
            Dataset = dataset;
        }

        [HttpGet]
        public IEnumerable<Route> List()
        {
            Logger.LogInformation(nameof(List));
            return Dataset.Routes;
        }
    }
}
