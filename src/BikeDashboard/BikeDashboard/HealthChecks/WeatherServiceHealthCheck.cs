using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BikeDashboard.Models;
using BikeDashboard.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BikeDashboard.HealthChecks
{
	public class WeatherServiceHealthCheck : IHealthCheck
    {
		private readonly IWeatherService _weatherService;
		private readonly StationCoordinates _testableCoordinates;
		private const double _latitude = 40.730610; //New York City
		private const double _longitude = -73.935242; 
       
		public WeatherServiceHealthCheck(IWeatherService weatherService)
        {
			_weatherService = weatherService;
			_testableCoordinates = new StationCoordinates { Latitude = _latitude, Longitude = _longitude };
		}

		public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
		{
			try
			{
				var forcast = await _weatherService.GetDailyForeCastAsync(_testableCoordinates);

				if(forcast.Forecasts.Any())
				{
					return HealthCheckResult.Healthy();
				}
				return HealthCheckResult.Degraded();
			}
			catch(Exception ex)
			{
				return HealthCheckResult.Degraded(nameof(IWeatherService), ex);
			}
		}
	}
}
