using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BikeDashboard.Models;
using BikeshareClient;
using System.Linq;

namespace BikeDashboard.Controllers
{
    [Route("api/[controller]")]
    public class SystemStatusController : Controller
    {
        private readonly IBikeshareClient _bikeshareClient;

        public SystemStatusController(IBikeshareClient bikeshareClient)
        {
            _bikeshareClient = bikeshareClient;
        }

        [HttpGet]
        public async Task<SystemStatus> GetAsync()
        {
            var allStations = await _bikeshareClient.GetStationsAsync();
            var stationStatuses = await _bikeshareClient.GetStationsStatusAsync();
            var systemInfo = await _bikeshareClient.GetSystemInformationAsync();

            return new SystemStatus
            {
                SystemName = systemInfo.Name,
                SystemOperator = systemInfo.OperatorName,
                Stations = allStations.Count(),
                StationsWithAvailableBikes = stationStatuses.Count(s => s.BikesAvailable > 0),
                StationsWithAvailableLocks = stationStatuses.Count(s => s.DocksAvailable > 0),
                StationsInstalled = stationStatuses.Count(s => s.Installed),
                StationsRenting = stationStatuses.Count(s => s.Renting),
                StationsReturning = stationStatuses.Count(s => s.Returning)

            };

        }
    }
}
