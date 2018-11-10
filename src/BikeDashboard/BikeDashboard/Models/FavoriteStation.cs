using System;
namespace BikeDashboard.Models
{
	public class FavoriteStation
	{
        public FavoriteStation(string name, int availableBikes, int availableLocks, StationCoordinates stationCoordinates)
		{
			Name = name;
			AvailableBikes = availableBikes;
			AvailableLocks = availableLocks;
            StationCoordinates = stationCoordinates;
		}

		public string Name { get; private set; }
		public int AvailableBikes { get; private set; }
		public int AvailableLocks { get; private set; }
        public StationCoordinates StationCoordinates { get; private set; }
    }
}
