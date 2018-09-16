using System;
using System.Threading.Tasks;
using BikeDashboard.DTO;
using BikeDashboard.Models;

namespace BikeDashboard.Services
{
    public interface IWeatherService
    {
		bool FeatureEnabled { get; }
		Task<WeatherForecastReport> GetDailyForeCastAsync(StationCoordinates coordinates);
	}
}
