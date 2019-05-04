using System;
using System.Threading.Tasks;
using BikeDashboard.Models;
using Microsoft.Extensions.Caching.Memory;

namespace BikeDashboard.Services
{
    public class TimeCachedWeatherService : IWeatherService
    {
        private const double CachedMinutes = 10;
        private readonly IWeatherService _weatherService;
        private readonly IMemoryCache _memoryCache;

        public TimeCachedWeatherService(IWeatherService weatherService, IMemoryCache memoryCache)
        {
            _weatherService = weatherService;
            _memoryCache = memoryCache;
        }

        public bool FeatureEnabled => _weatherService.FeatureEnabled;

        public async Task<WeatherForecastReport> GetDailyForeCastAsync(StationCoordinates coordinates)
        {
            var forcastReport = await _memoryCache.GetOrCreateAsync((coordinates.Latitude, coordinates.Longitude).GetHashCode(), async report => 
            {
                report.SetAbsoluteExpiration(TimeSpan.FromMinutes(CachedMinutes));
                return await _weatherService.GetDailyForeCastAsync(coordinates);

            });
            return forcastReport;
        }
    }
}
