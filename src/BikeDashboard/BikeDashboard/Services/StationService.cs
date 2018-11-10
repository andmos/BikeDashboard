using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BikeDashboard.Models;
using BikeshareClient;
using BikeshareClient.Models;
namespace BikeDashboard.Services
{
	public class StationService : IStationService
    {
		private readonly string _stationName;
		private readonly IBikeshareClient _bikeShareClient; 

		public StationService(IBikeshareClient bikeshareClient, string defaultStationName)
        {
			_bikeShareClient = bikeshareClient;
			_stationName = defaultStationName; 
		}

		public async Task<FavoriteStation> GetFavoriteStation()
		{
			return await GetFavoriteStation(_stationName);   
		}

		public async Task<FavoriteStation> GetFavoriteStation(string stationName)
		{
			var stations = await _bikeShareClient.GetStationsAsync();
            var stationIdentifier = new KeyValuePair<string, string>();
            StationCoordinates stationCoordinates;
            try
            {
                stationIdentifier = new KeyValuePair<string, string>(
                stations.First(s => s.Name.ToLower().Equals(stationName.ToLower())).Id,
                stations.First(s => s.Name.ToLower().Equals(stationName.ToLower())).Name);
                stationCoordinates = new StationCoordinates
                {
                    Latitude = stations.First(s => s.Name.ToLower().Equals(stationName.ToLower())).Latitude,
                    Longitude = stations.First(s => s.Name.ToLower().Equals(stationName.ToLower())).Longitude
                };
            }
            catch(InvalidOperationException noElementException)
            {
                return await GetFavoriteStation(_stationName);
            }
            var stationsStatuses = await _bikeShareClient.GetStationsStatusAsync();
			var stationStatus = stationsStatuses.First(s => s.Id.Equals(stationIdentifier.Key));

			return new FavoriteStation(stationIdentifier.Value, stationStatus.BikesAvailable, stationStatus.DocksAvailable, stationCoordinates); 	
		}

        public async Task<IEnumerable<Station>> GetAllAvailableStations()
        {
            return await _bikeShareClient.GetStationsAsync();
        }

        public async Task<FavoriteStation> GetClosestAvailableStation(FavoriteStation baseStation)
        {
            var baseStationCoordinates = baseStation.StationCoordinates;
            var stationsStatuses = await _bikeShareClient.GetStationsStatusAsync();
            var emptyStations = new HashSet<string>(stationsStatuses.Where(s => s.BikesAvailable == 0).Select(n => n.Id));
            var stations = await GetAllAvailableStations();
            var stationList = stations.ToList();
            stationList.RemoveAll(s => emptyStations.Contains(s.Id));

            var closestToBaseStation = stationList.OrderBy(s => (GeoLocation.GeoCalculator.GetDistance(baseStationCoordinates.Latitude, baseStationCoordinates.Longitude, s.Latitude, s.Longitude, 2, GeoLocation.DistanceUnit.Meters))).First();
            return await GetFavoriteStation(closestToBaseStation.Name);

        }

    }
}
