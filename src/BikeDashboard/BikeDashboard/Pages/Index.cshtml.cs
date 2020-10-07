using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeDashboard.Models;
using BikeDashboard.ViewModels;
using BikeDashboard.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BikeDashboard.Pages
{
	public class IndexModel : PageModel
	{
		private readonly IStationService _stationService;
		private readonly IWeatherService _weatherService;

		public IndexModel(IStationService stationService, IWeatherService weatherService)
		{
			_stationService = stationService;
			_weatherService = weatherService;
			WeatherServiceEnabled = _weatherService.FeatureEnabled;
		}

		[BindProperty]
		public bool WeatherServiceEnabled {get; set; }

		[BindProperty]
		public FavoriteStation FavoriteStation { get; set; }

        [BindProperty]
        public string InfoText { get; set; }
        
		public IEnumerable<WeatherForecastViewModel> WeatherForecastViewModels { get; set; }
      
		public async Task OnGetAsync()
        {
            InfoText = string.Empty;
			var stationQueryString = HttpContext.Request.Query["stationName"].ToString();
			if(string.IsNullOrEmpty(stationQueryString))
			{
				FavoriteStation = await _stationService.GetFavoriteStation();	
			}
			else
			{
				FavoriteStation = await _stationService.GetFavoriteStation(stationQueryString);
			}

            if (FavoriteStation.AvailableBikes == 0)
            {
                InfoText = $"{FavoriteStation.Name} had no available bikes. Showing closest bikestation."; 
                FavoriteStation = await _stationService.GetClosestAvailableStation(FavoriteStation);
            }

			if(WeatherServiceEnabled)
			{
				WeatherForecastReport weatherForecastReport;
                try
                {
                    weatherForecastReport = await _weatherService.GetDailyForeCastAsync(FavoriteStation.StationCoordinates);
                    WeatherForecastViewModels = GetForecastViewModels(weatherForecastReport.Forecasts);
                }
                catch(NotImplementedException exception)
                {
                    WeatherServiceEnabled = false;
                }

			}
        }

		private IEnumerable<WeatherForecastViewModel> GetForecastViewModels(IEnumerable<WeatherForecast> forecasts)
		{
			var forecastViewModels = new List<WeatherForecastViewModel>();
			for (int i = 0; i < forecasts.Count() -1; i++)
			{
				var forecastViewModel = new WeatherForecastViewModel(forecasts.ToArray()[i], forecasts.ToArray()[i + 1].ForecastTime);
				forecastViewModels.Add(forecastViewModel);
			}
			return forecastViewModels;
		}
    }
}
