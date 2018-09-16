using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BikeDashboard.DTO;
using BikeDashboard.Models;
using Newtonsoft.Json;
using System.Linq;

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
			_weatherServiceAPIKey = weatherServiceAPIKey;
		}

		public bool FeatureEnabled => !String.IsNullOrWhiteSpace(_weatherServiceAPIKey);

		public async Task<WeatherForecastReport> GetDailyForeCastAsync(StationCoordinates coordinates)
		{
			using(var client = new HttpClient())
			{
				var response = await client.GetAsync($"{_apiBaseAddress}?" +
				                                     $"lat={coordinates.Latitude}" +
				                                     $"&lon={coordinates.Longitude}" +
				                                     $"&APPID={_weatherServiceAPIKey}" +
				                                     $"&units={_tempUnit}" +
				                                     $"&cnt={_numberOfForecastRecords}");
				if (!response.IsSuccessStatusCode)
                {
					throw new NotImplementedException($"Could not find any weather data, {_apiBaseAddress} returned status code {response.StatusCode}");
                }
				var content = await response.Content.ReadAsStringAsync();
                
				return CreateWeatherForecastReport(JsonConvert.DeserializeObject<WeatherReportDTO>(content));
			}
		}



		private WeatherForecastReport CreateWeatherForecastReport(WeatherReportDTO weatherReportDto)
		{
			var forecasts = new List<WeatherForecast>(); 

			foreach(var forecastDto in weatherReportDto.list)
			{
				var rain = new Models.Rain(forecastDto.rain?.Rainfall ?? 0);
				var temperature = new Temperature(forecastDto.main.temp_min, forecastDto.main.temp_max, forecastDto.main.humidity);
				var wind = new Models.Wind(forecastDto.wind.speed);
				var forecast = new WeatherForecast(rain, 
				                                   temperature, 
				                                   wind, 
				                                   forecastDto.weather.FirstOrDefault().description, 
				                                   forecastDto.weather.FirstOrDefault().main,
				                                   forecastDto.RecordTime);
				forecasts.Add(forecast);
			}

			return new WeatherForecastReport(forecasts);
		}
    }
}
