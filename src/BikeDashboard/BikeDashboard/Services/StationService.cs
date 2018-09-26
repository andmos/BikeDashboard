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
            try
            {
                stationIdentifier = new KeyValuePair<string, string>(
                stations.First(s => s.Name.ToLower().Equals(stationName.ToLower())).Id,
                stations.First(s => s.Name.ToLower().Equals(stationName.ToLower())).Name);
            }
            catch(InvalidOperationException noElementException)
            {
                return await GetFavoriteStation(_stationName);
            }
            var stationsStatuses = await _bikeShareClient.GetStationsStatusAsync();
			var stationStatus = stationsStatuses.First(s => s.Id.Equals(stationIdentifier.Key));

			return new FavoriteStation(stationIdentifier.Value, stationStatus.BikesAvailable, stationStatus.DocksAvailable); 	
		}

		public async Task<StationCoordinates> GetFavoriteStationCoordinates()
		{
			return await GetFavoriteStationCoordinates(_stationName);
		}

		public async Task<StationCoordinates> GetFavoriteStationCoordinates(string stationName)
		{
			var stations = await _bikeShareClient.GetStationsAsync(); 
			var station = stations.First(s => s.Name.Equals(stationName));
			return new StationCoordinates{ Latitude = station.Latitude, Longitude = station.Longitude};
		}
        
        public async Task<IEnumerable<Station>> GetAllAvailableStations()
        {
            return await _bikeShareClient.GetStationsAsync();
        }
	}
}
