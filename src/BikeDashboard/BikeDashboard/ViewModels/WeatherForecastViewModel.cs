using System;
using BikeDashboard.Models;
namespace BikeDashboard.ViewModels
{
    public class WeatherForecastViewModel
    {
		public Rain Rain { get; }
        public Temperature Temperature { get; }
        public Wind Wind { get; }
        public String Description { get; }
		public String ForecastStartTime { get; } 
		public String ForecastEndTime { get; }
		private String _weatherGroup;

		public WeatherForecastViewModel(WeatherForecast forecast, DateTime nextForcastDate)
        {
			Rain = forecast.Rain;
			Temperature = forecast.Temperature;
			Wind = forecast.Wind;
			Description = forecast.Description;
			ForecastStartTime = forecast.ForecastTime.ToLocalTime().ToShortTimeString();
			_weatherGroup = forecast.WeatherGroup;
			ForecastEndTime = nextForcastDate.ToLocalTime().ToShortTimeString();

		}
    }
}
