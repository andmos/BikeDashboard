using System;
using BikeDashboard.Models;
namespace BikeDashboard.ViewModels
{
    public class WeatherForecastViewModel
    {
		public Rainfall Rain { get; }
        public Temperature Temperature { get; }
        public Wind Wind { get; }
        public String Description { get; }
		public String ForecastStartTime { get; } 
		public String ForecastEndTime { get; }
		private String _weatherGroup;

		private const int _decimalAccuracy = 1; 

		public WeatherForecastViewModel(WeatherForecast forecast, DateTime nextForcastDate)
        {
			Rain = new Rainfall(Math.Round(forecast.Rain.Value, _decimalAccuracy));
			Temperature = new Temperature(
				Math.Round(forecast.Temperature.Min, _decimalAccuracy), 
				Math.Round(forecast.Temperature.Max, _decimalAccuracy), 
				forecast.Temperature.Humidity);

			Wind = new Wind(Math.Round(forecast.Wind.Speed,_decimalAccuracy));
			Description = forecast.Description;
			ForecastStartTime = forecast.ForecastTime.ToLocalTime().ToShortTimeString();
			_weatherGroup = forecast.WeatherGroup;
			ForecastEndTime = nextForcastDate.ToLocalTime().ToShortTimeString();

		}
    }
}
