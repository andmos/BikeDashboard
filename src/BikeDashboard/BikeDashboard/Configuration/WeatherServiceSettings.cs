using System;
namespace BikeDashboard.Configuration
{
    public class WeatherServiceSettings
    {
        public string WeatherServiceAPIKey { get; set; }
        public Uri ApiBaseAddress { get; set; } = new Uri("https://api.openweathermap.org/data/2.5/forecast");
    }
}
