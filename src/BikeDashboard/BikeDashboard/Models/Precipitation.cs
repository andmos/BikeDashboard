using System;
namespace BikeDashboard.Models
{
    public class Precipitation
    {
        public double Value { get; set; }
        public PrecipitationType PrecipitationType { get; set; }
		public Precipitation(double rainfallValue, PrecipitationType precipitationType)
        {
			Value = rainfallValue;
            PrecipitationType = precipitationType;
		}
    }

    public enum PrecipitationType
    {
        Rain,
        Snow
    }
}
