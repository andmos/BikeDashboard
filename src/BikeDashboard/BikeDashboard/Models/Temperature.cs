using System;
namespace BikeDashboard.Models
{
    public class Temperature
    {
		public double Min { get; }
		public double Max { get; }
		public int Humidity { get; } 

		public Temperature(double min, double max, int humidity)
        {
			Min = min;
			Max = max;
			Humidity = humidity;
		}
    }
}
