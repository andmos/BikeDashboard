using System.Threading.Tasks;
using BikeDashboard.DTO;
using BikeDashboard.Models;
using BikeDashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace BikeDashboard.Controllers
{
    [Route("api/[controller]")]
    public class FavoriteStationController : Controller
    {
		private readonly IStationService _stationService;
		private readonly IWeatherService _weatherService; 
        
		public FavoriteStationController(IStationService stationService, IWeatherService weatherService)
		{
			_stationService = stationService;
			_weatherService = weatherService;
		}
        
  //      [HttpGet]
		//public async Task<FavoriteStation> GetAsync([FromQuery(Name = "stationName")] string stationName)
   //     {
			//if(string.IsNullOrWhiteSpace(stationName))
			//{
			//	return await _stationService.GetFavoriteStation();	
			//}
			//return await _stationService.GetFavoriteStation(stationName);
        //}

		[HttpGet]
		public async Task<WeatherForecastReport> GetAsync([FromQuery(Name = "stationName")] string stationName, [FromQuery(Name = "weather")] string weather)
		{
			if(bool.Parse(weather))
			{
				StationCoordinates coordinates;
                if (string.IsNullOrWhiteSpace(stationName))
                {
                    coordinates = await _stationService.GetFavoriteStationCoordinates();
                }

                coordinates = await _stationService.GetFavoriteStationCoordinates(stationName);
				return await _weatherService.GetDailyForeCastAsync(coordinates);
			}
			return new WeatherForecastReport(new WeatherForecast[]{});

		}
        
    }
}
