using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BikeDashboard.DTO;
using BikeDashboard.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using BikeDashboard.Configuration;
using Microsoft.Extensions.Options;

namespace BikeDashboard.Services
{
	public class WeatherService : IWeatherService
	{
		private readonly Uri _apiBaseAddress;
        private readonly int _servicePointLeaseTime = 60000;
        private readonly string _weatherServiceApiKey;
		private readonly string _tempUnit = "metric";
		private readonly int _numberOfForecastRecords = 4; // 3 hours between forecasts
        private readonly HttpClient _httpClient;

        public WeatherService(IOptions<WeatherServiceSettings> weatherServiceSettings, IHttpClientFactory httpClientFactory)
        {
            _weatherServiceApiKey = weatherServiceSettings.Value.WeatherServiceApiKey;
            _apiBaseAddress = weatherServiceSettings.Value.ApiBaseAddress;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = _apiBaseAddress;

            ServicePointManager.FindServicePoint(_apiBaseAddress).ConnectionLeaseTimeout = _servicePointLeaseTime;
        }

		public bool FeatureEnabled => !string.IsNullOrWhiteSpace(_weatherServiceApiKey);

		public async Task<WeatherForecastReport> GetDailyForeCastAsync(StationCoordinates coordinates)
		{
            var response = await _httpClient.GetAsync($"?lat={coordinates.Latitude}" +
				                                     $"&lon={coordinates.Longitude}" +
				                                     $"&APPID={_weatherServiceApiKey}" +
				                                     $"&units={_tempUnit}" +
				                                     $"&cnt={_numberOfForecastRecords}");
            if (!response.IsSuccessStatusCode)
            {
				throw new NotImplementedException($"Could not find any weather data, {_apiBaseAddress} returned status code {response.StatusCode}");
            }
			var content = await response.Content.ReadAsStringAsync();
                
			return CreateWeatherForecastReport(JsonConvert.DeserializeObject<WeatherReportDTO>(content));
			
		}


		private WeatherForecastReport CreateWeatherForecastReport(WeatherReportDTO weatherReportDto)
		{
			var forecasts = new List<WeatherForecast>(); 

			foreach(var forecastDto in weatherReportDto.list)
            {
	            var precipitation = new Precipitation(ParsePrecipitationQuantity(forecastDto), ParsePrecipitationType(forecastDto));
                var temperature = new Temperature(forecastDto.main.temp_min, forecastDto.main.temp_max, forecastDto.main.humidity);
                var wind = new Models.Wind(forecastDto.wind.speed);
                var forecast = new WeatherForecast(precipitation,
                                                   temperature,
                                                   wind,
                                                   forecastDto.weather.FirstOrDefault()?.description,
                                                   forecastDto.weather.FirstOrDefault()?.main,
                                                   forecastDto.RecordTime);
                forecasts.Add(forecast);
            }

            return new WeatherForecastReport(forecasts);
		}

        private static double ParsePrecipitationQuantity(Forecast forecastDto)
        {
            return forecastDto.rain?.Rainfall ?? forecastDto.snow?.Snowfall ?? 0;
        }

        private static PrecipitationType ParsePrecipitationType(Forecast weatherForecastDto) 
        {
            return weatherForecastDto.rain?.Rainfall != null 
                ? PrecipitationType.Rain : weatherForecastDto.snow?.Snowfall != null 
                ? PrecipitationType.Snow : PrecipitationType.Rain;
        }
    }
}
