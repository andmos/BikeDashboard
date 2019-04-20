using System;
using System.Collections.Generic;

namespace BikeDashboard.Models
{
    public class WeatherForecastReport
    {
		public IEnumerable<WeatherForecast> Forecasts { get; }

		public WeatherForecastReport(IEnumerable<WeatherForecast> forecasts)
        {
			Forecasts = forecasts;
		}
    }
}
