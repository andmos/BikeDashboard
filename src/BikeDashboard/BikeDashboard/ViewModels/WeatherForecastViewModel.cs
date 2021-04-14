using System;
using BikeDashboard.Models;
namespace BikeDashboard.ViewModels
{
    public class WeatherForecastViewModel
    {
		public Precipitation Rain { get; }
        public Temperature Temperature { get; }
        public Wind Wind { get; }
        public string Description { get; }
		public string ForecastStartTime { get; } 
		public string ForecastEndTime { get; }


        private const int _decimalAccuracy = 1; 

		public WeatherForecastViewModel(WeatherForecast forecast, DateTime nextForcastDate)
        {
			Rain = new Precipitation(Math.Round(forecast.Precipitation.Value, _decimalAccuracy), forecast.Precipitation.PrecipitationType);
			Temperature = new Temperature(
				Math.Round(forecast.Temperature.Min, _decimalAccuracy), 
				Math.Round(forecast.Temperature.Max, _decimalAccuracy), 
				forecast.Temperature.Humidity);

			Wind = new Wind(Math.Round(forecast.Wind.Speed,_decimalAccuracy));
			Description = forecast.Description;
			ForecastStartTime = forecast.ForecastTime.ToLocalTime().ToShortTimeString();
			ForecastEndTime = nextForcastDate.ToLocalTime().ToShortTimeString();

		}


    }
}
