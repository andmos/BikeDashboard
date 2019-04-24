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
        private readonly string _defaultStationName;
        private readonly IBikeshareClient _bikeShareClient;

        public StationService(IBikeshareClient bikeshareClient, string defaultStationName)
        {
            _bikeShareClient = bikeshareClient;
            _defaultStationName = defaultStationName;
        }
        
        public async Task<FavoriteStation> GetFavoriteStation()
        {
            return await GetFavoriteStation(_defaultStationName);
        }

        public async Task<FavoriteStation> GetFavoriteStation(string stationName)
        {
            var stations = await _bikeShareClient.GetStationsAsync();
            StationIdentity stationIdentifier;
            StationCoordinates stationCoordinates;
            try
            {
                stationIdentifier = new StationIdentity(
                    stations.First(s => s.Name.ToLower().Equals(stationName.ToLower())).Name,
                    stations.First(s => s.Name.ToLower().Equals(stationName.ToLower())).Id
                    );
                stationCoordinates = new StationCoordinates
                {
                    Latitude = stations.First(s => s.Name.ToLower().Equals(stationName.ToLower())).Latitude,
                    Longitude = stations.First(s => s.Name.ToLower().Equals(stationName.ToLower())).Longitude
                };
            }
            catch (InvalidOperationException noElementException)
            {
                if (!stationName.Equals(_defaultStationName)) 
                {
                    return await GetFavoriteStation(_defaultStationName);
                }
                var bikeSystemInfo = await _bikeShareClient.GetSystemInformationAsync();
                throw new ArgumentException($"Can't find {_defaultStationName} station in bikeshare-provider {bikeSystemInfo.Name} ({bikeSystemInfo.OperatorName})");
            }
            var stationsStatuses = await _bikeShareClient.GetStationsStatusAsync();
            var stationStatus = stationsStatuses.First(s => s.Id.Equals(stationIdentifier.Id));

            return new FavoriteStation(stationIdentifier.Name, stationStatus.BikesAvailable, stationStatus.DocksAvailable, stationCoordinates);
        }

        public async Task<IEnumerable<Station>> GetAllAvailableStations()
        {
            return await _bikeShareClient.GetStationsAsync();
        }

        public async Task<FavoriteStation> GetClosestAvailableStation(FavoriteStation baseStation)
        {
            var baseStationCoordinates = baseStation.StationCoordinates;
            var stationList = await RemoveEmptyStations();
            if (!stationList.Any())
            {
                return new FavoriteStation("No Available Bikes found from bikeshare provider.", 0, 0, baseStationCoordinates);
            }

            var closestToBaseStation = stationList.OrderBy(
                s => 
                    (GeoLocation.GeoCalculator.GetDistance(
                        baseStationCoordinates.Latitude, 
                        baseStationCoordinates.Longitude, 
                        s.Latitude, s.Longitude, 2, GeoLocation.DistanceUnit.Meters)
                    ))
                    .First();
            return await GetFavoriteStation(closestToBaseStation.Name);

        }

        private async Task<IEnumerable<Station>> RemoveEmptyStations()
        {
            var stationsStatuses = await _bikeShareClient.GetStationsStatusAsync();
            var emptyStations = new HashSet<string>(stationsStatuses.Where(
                s => s.BikesAvailable == 0 || !s.Renting).Select(n => n.Id));
            var stations = await GetAllAvailableStations();
            var stationList = stations.ToList();
            stationList.RemoveAll(s => emptyStations.Contains(s.Id));
            return stationList;
        }

    }
}
