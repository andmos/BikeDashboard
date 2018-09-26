using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeDashboard.Models;
using BikeshareClient.Models;

namespace BikeDashboard.Services
{
	public interface IStationService
    {
		Task<FavoriteStation> GetFavoriteStation();
		Task<FavoriteStation> GetFavoriteStation(string stationName);
		Task<StationCoordinates> GetFavoriteStationCoordinates();
		Task<StationCoordinates> GetFavoriteStationCoordinates(string stationName);
		Task<IEnumerable<Station>> GetAllAvailableStations();
    }
}
