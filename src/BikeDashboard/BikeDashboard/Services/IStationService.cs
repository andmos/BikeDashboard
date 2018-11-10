using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BikeDashboard.Models;
using BikeshareClient.Models;

namespace BikeDashboard.Services
{
	public interface IStationService
    {
        /// <summary>
        /// Gets the favorite station configured by constructor
        /// </summary>
        /// <returns>The favorite station.</returns>
		Task<FavoriteStation> GetFavoriteStation();
        /// <summary>
        /// Gets the favorite station from input station name
        /// </summary>
        /// <returns>The favorite station.</returns>
        /// <param name="stationName">Station name.</param>
		Task<FavoriteStation> GetFavoriteStation(string stationName);
        /// <summary>
        /// Gets all available stations.
        /// </summary>
        /// <returns>The all available stations.</returns>
		Task<IEnumerable<Station>> GetAllAvailableStations();
        /// <summary>
        /// Gets the closest station to another station.
        /// </summary>
        /// <returns>The closest station.</returns>
        /// <param name="baseStation">Base station.</param>
        Task<FavoriteStation> GetClosestAvailableStation(FavoriteStation baseStation);
    }
}
