using System.Collections.Generic;
using System.Threading.Tasks;
using BikeDashboard.DTO;
using BikeDashboard.Models;
using System.Linq;
using BikeDashboard.Configuration;
using Microsoft.Extensions.Options;

namespace BikeDashboard.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly string _weatherServiceApiKey;
        private readonly OpenWeatherMapClient _weatherMapClient;

        public WeatherService(
            IOptions<WeatherServiceSettings> weatherServiceSettings, 
            OpenWeatherMapClient weatherMapClient)
        {
            _weatherMapClient = weatherMapClient;
            _weatherServiceApiKey = weatherServiceSettings.Value.WeatherServiceApiKey;
        }

        public bool FeatureEnabled => !string.IsNullOrWhiteSpace(_weatherServiceApiKey);

        public async Task<WeatherForecastReport> GetDailyForeCastAsync(StationCoordinates coordinates)
        {
            return CreateWeatherForecastReport(await _weatherMapClient.GetDailyForeCastAsync(coordinates));
        }
        
        private WeatherForecastReport CreateWeatherForecastReport(WeatherReportDTO weatherReportDto)
        {
            var forecasts = new List<WeatherForecast>();

            foreach (var forecastDto in weatherReportDto.list)
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
