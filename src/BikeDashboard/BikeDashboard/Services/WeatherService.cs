using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BikeDashboard.DTO;
using BikeDashboard.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace BikeDashboard.Services
{
	public class WeatherService : IWeatherService
    {
        private readonly Uri ApiBaseAddress = new Uri("https://api.openweathermap.org/data/2.5/forecast");
        private readonly int _servicePointLeaseTime = 60000;
        private readonly string _weatherServiceAPIKey;
		private readonly string _tempUnit = "metric";
		private readonly int _numberOfForecastRecords = 4; // 3 hours between forecasts
        private readonly HttpClient _httpClient;

        public WeatherService(string weatherServiceAPIKey)
        {
            _weatherServiceAPIKey = weatherServiceAPIKey;
            _httpClient = new HttpClient()
            {
                BaseAddress = ApiBaseAddress,
            };
            ServicePointManager.FindServicePoint(ApiBaseAddress).ConnectionLeaseTimeout = _servicePointLeaseTime;

        }

		public bool FeatureEnabled => !string.IsNullOrWhiteSpace(_weatherServiceAPIKey);

		public async Task<WeatherForecastReport> GetDailyForeCastAsync(StationCoordinates coordinates)
		{
            var response = await _httpClient.GetAsync($"?lat={coordinates.Latitude}" +
				                                     $"&lon={coordinates.Longitude}" +
				                                     $"&APPID={_weatherServiceAPIKey}" +
				                                     $"&units={_tempUnit}" +
				                                     $"&cnt={_numberOfForecastRecords}");
            if (!response.IsSuccessStatusCode)
            {
				throw new NotImplementedException($"Could not find any weather data, {ApiBaseAddress} returned status code {response.StatusCode}");
            }
			var content = await response.Content.ReadAsStringAsync();
                
			return CreateWeatherForecastReport(JsonConvert.DeserializeObject<WeatherReportDTO>(content));
			
		}


		private WeatherForecastReport CreateWeatherForecastReport(WeatherReportDTO weatherReportDto)
		{
			var forecasts = new List<WeatherForecast>(); 

			foreach(var forecastDto in weatherReportDto.list)
			{
				var rain = new Rainfall(forecastDto.rain?.Rainfall ?? forecastDto.snow?.Snowfall ?? 0);
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
