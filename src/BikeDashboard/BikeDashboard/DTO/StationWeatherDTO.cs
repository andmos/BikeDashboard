using System;
using BikeDashboard.Models;

namespace BikeDashboard.DTO
{
	public class StationWeatherDTO
	{
		public FavoriteStation Station { get; set; }
		public WeatherForecastReport ForecastReport { get; set; }
	}
}