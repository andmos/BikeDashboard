using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BikeDashboard.DTO
{
	public class Main
    {
        public double temp { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double pressure { get; set; }
        public double sea_level { get; set; }
        public double grnd_level { get; set; }
        public int humidity { get; set; }
        public double temp_kf { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public double deg { get; set; }
    }

    public class Rain
    {
		[JsonProperty("3h")]
		public double? Rainfall { get; set; }
    }

    public class Snow
    {
        [JsonProperty("3h")]
        public double? Snowfall { get; set; }
    }

    public class Sys
    {
        public string pod { get; set; }
    }

    public class Forecast
    {
		[JsonConverter(typeof(UnixDateTimeConverter)), JsonProperty("dt")]
		public DateTime RecordTime { get; set; }
        
		public Main main { get; set; }
        public List<Weather> weather { get; set; }
        public Clouds clouds { get; set; }
        public Wind wind { get; set; }
        public Rain rain { get; set; }
        public Snow snow { get; set; }
        public Sys sys { get; set; }
        public string dt_txt { get; set; }
    }
 
    public class WeatherReportDTO
    {
        public string cod { get; set; }
        public double message { get; set; }
		[JsonProperty("cnt")]
		public int NumberOfForecasts { get; set; }
        public List<Forecast> list { get; set; }
       
    }
}
