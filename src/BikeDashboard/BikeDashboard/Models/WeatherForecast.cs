using System;
namespace BikeDashboard.Models
{
	public class WeatherForecast
	{
		public Rainfall Rain {get;}
		public Temperature Temperature { get; }
		public Wind Wind { get; }
		public String Description { get; }
		public String WeatherGroup { get; }
		public DateTime ForecastTime { get; }

		public WeatherForecast(Rainfall rain, Temperature temperature, Wind wind, String description, string weatherGroup, DateTime forecastTime)
        {
			Rain = rain;
			Temperature = temperature;
			Wind = wind;
			Description = description;
			WeatherGroup = weatherGroup;
			ForecastTime = forecastTime;
		}
    }
}
