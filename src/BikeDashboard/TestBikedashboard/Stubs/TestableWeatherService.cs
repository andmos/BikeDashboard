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
                    new Rainfall(2.0),
                    new Temperature(2, 5, 2), 
                    new Wind(10), 
                    "Light Rain", 
                    "Boring weather", 
                    new DateTime(1979, 07, 28, 22, 35, 5, 
                    DateTimeKind.Utc))
            });

            return await Task.FromResult(weatherForcastReport);
        }
    }
}
