using System;
using System.Linq;
using System.Threading.Tasks;
using BikeDashboard.Models;
using BikeshareClient;
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
			var stationId = stations.First(s => s.Name.Equals(stationName)).Id;
            var stationsStatuses = await _bikeShareClient.GetStationsStatusAsync();
            var stationStatus = stationsStatuses.First(s => s.Id.Equals(stationId));

			return new FavoriteStation(stationName, stationStatus.BikesAvailable, stationStatus.DocksAvailable); 	
		}
	}
}
