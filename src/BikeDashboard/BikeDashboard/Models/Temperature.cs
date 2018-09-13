using System;
namespace BikeDashboard.Models
{
    public class Temperature
    {
		public Double Min { get; }
		public Double Max { get; }
		public int Humidity { get; } 

		public Temperature(double min, double max, int humidity)
        {
			Min = min;
			Max = max;
			Humidity = humidity;
		}
    }
}
