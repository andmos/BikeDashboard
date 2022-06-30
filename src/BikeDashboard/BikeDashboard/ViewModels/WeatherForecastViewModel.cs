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


        private const int DecimalAccuracy = 0;

        public WeatherForecastViewModel(WeatherForecast forecast, DateTime nextForecastDate)
        {
            Rain = new Precipitation(Math.Round(forecast.Precipitation.Value, DecimalAccuracy), forecast.Precipitation.PrecipitationType);
            Temperature = new Temperature(
                Math.Round(forecast.Temperature.Min, DecimalAccuracy),
                Math.Round(forecast.Temperature.Max, DecimalAccuracy),
                forecast.Temperature.Humidity);

            Wind = new Wind(Math.Round(forecast.Wind.Speed, DecimalAccuracy));
            Description = forecast.Description;
            ForecastStartTime = forecast.ForecastTime.ToLocalTime().ToShortTimeString();
            ForecastEndTime = nextForecastDate.ToLocalTime().ToShortTimeString();

        }
    }
}
