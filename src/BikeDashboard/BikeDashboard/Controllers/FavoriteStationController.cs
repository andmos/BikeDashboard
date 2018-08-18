using System.Threading.Tasks;
using BikeDashboard.Models;
using BikeDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikeDashboard.Controllers
{
    [Route("api/[controller]")]
    public class FavoriteStationController : Controller
    {
		private readonly IStationService _stationService;
        
		public FavoriteStationController(IStationService stationService)
		{
			_stationService = stationService; 
		}
        
        [HttpGet]
		public async Task<FavoriteStation> GetAsync()
        {
			return await _stationService.GetFavoriteStation();
        }
        
    }
}
