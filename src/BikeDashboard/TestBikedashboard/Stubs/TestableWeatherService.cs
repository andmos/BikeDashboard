using System;
using System.Threading.Tasks;
using BikeDashboard.Models;
using BikeDashboard.Services;

namespace TestBikedashboard.Stubs
{
    public class TestableWeatherService : IWeatherService 
    {

        public bool FeatureEnabled => true;

        public async Task<WeatherForecastReport> GetDailyForeCastAsync(StationCoordinates coordinates)
        {
            var weatherForcastReport = new WeatherForecastReport(
            new[] 
                { new WeatherForecast(
                    new Precipitation(2.0, PrecipitationType.Rain),
                    new Temperature(2, 5, 2), 
                    new Wind(10), 
                    "Light Rain", 
                    "Boring weather", 
                    new DateTime(1979, 07, 28, 22, 35, 5, 
                    DateTimeKind.Utc)), 

                  new WeatherForecast(
                    new Precipitation(2.0, PrecipitationType.Snow),
                    new Temperature(2, 5, 2),
                    new Wind(10),
                    "Light Snow",
                    "Snowy weather",
                    new DateTime(1979, 07, 28, 22, 40, 5,
                    DateTimeKind.Utc))

            });

            return await Task.FromResult(weatherForcastReport);
        }
    }
}
