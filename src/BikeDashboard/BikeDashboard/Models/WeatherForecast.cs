using System;
namespace BikeDashboard.Models
{
	public class WeatherForecast
	{
		public Rain Rain {get;}
		public Temperature Temperature { get; }
		public Wind Wind { get; }
		public String Description { get; }
		public String WeatherGroup { get; set; }
		public DateTime ForecastTime { get; set; }

		public WeatherForecast(Rain rain, Temperature temperature, Wind wind, String description, string weatherGroup, DateTime forecastTime)
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
