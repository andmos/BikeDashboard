using System;
namespace BikeDashboard.Models
{
    public class Rainfall
    {
		public double Value { get; set; }
		public Rainfall(double rainfallValue)
        {
			Value = rainfallValue;
		}
    }
}
