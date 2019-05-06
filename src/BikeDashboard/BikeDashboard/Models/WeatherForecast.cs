using System;
namespace BikeDashboard.Models
{
	public class WeatherForecast
	{
		public Precipitation Precipitation {get;}
		public Temperature Temperature { get; }
		public Wind Wind { get; }
		public string Description { get; }
		public string WeatherGroup { get; }
		public DateTime ForecastTime { get; }

		public WeatherForecast(Precipitation precipitation, Temperature temperature, Wind wind, string description, string weatherGroup, DateTime forecastTime)
        {
			Precipitation = precipitation;
			Temperature = temperature;
			Wind = wind;
			Description = description;
			WeatherGroup = weatherGroup;
			ForecastTime = forecastTime;
		}
    }
}
