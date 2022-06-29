using System;
using BikeDashboard.Models;
using Newtonsoft.Json;

namespace BikeDashboard.DTO
{
    public class StationWeatherDTO
    {
        public FavoriteStation Station { get; set; }

        [JsonProperty("forecastReport")]
        public WeatherForecastReport ForecastReport { get; set; }
    }
}