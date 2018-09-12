using System;
using System.Net.Http;
using System.Threading.Tasks;
using BikeDashboard.Models;

namespace BikeDashboard.Services
{
	public class WeatherService : IWeatherService
    {
		private readonly string _weatherServiceAPIKey;
		private readonly string _apiBaseAddress = "https://api.openweathermap.org/data/2.5/forecast";
		private readonly string _tempUnit = "metric";
		private readonly int _numberOfForecastRecords = 4; // 3 hours between forecasts
        
		public WeatherService(string weatherServiceAPIKey)
        {
			if(string.IsNullOrEmpty(weatherServiceAPIKey))
			{
				throw new ArgumentNullException("weatherServiceAPIKey must be set to call openweathermap services");
			}
			_weatherServiceAPIKey = weatherServiceAPIKey;
		}

		public async Task<string> GetDailyForeCastAsync(StationCoordinates coordinates)
		{
			using(var client = new HttpClient())
			{
				var response = await client.GetAsync($"{_apiBaseAddress}?" +
				                                     "lat={coordinates.Latitude}" +
				                                     "&lon={coordinates.Longitude}" +
				                                     "&APPID={_weatherServiceAPIKey}" +
				                                     "&units={_tempUnit}" +
				                                     "&cnt={_numberOfForecastRecords}");
				if (!response.IsSuccessStatusCode)
                {
					throw new NotImplementedException($"Could not find any weather data, {_apiBaseAddress} returned status code {response.StatusCode}");
                }
				var content = await response.Content.ReadAsStringAsync();
				return content; 
			}
		}
    }
}
