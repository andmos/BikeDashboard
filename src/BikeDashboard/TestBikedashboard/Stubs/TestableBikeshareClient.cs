using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BikeshareClient;
using BikeshareClient.Models;
using Newtonsoft.Json;
using TestBikedashboard.DTO;

namespace TestBikedashboard.Stubs
{
    public class TestableBikeshareClient : IBikeshareClient
    {

        public Task<IEnumerable<Feed>> GetAvailableFeedsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Language>> GetAvailableLanguagesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BikeStatus>> GetBikeStatusAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Station>> GetStationsAsync()
        {
            var stationsJson = await File.ReadAllTextAsync("TestData/BikeStations.json");
            var dto = JsonConvert.DeserializeObject<StationDTO>(stationsJson);
            return dto.StationsData.Stations;
        }

        public async Task<IEnumerable<StationStatus>> GetStationsStatusAsync()
        {
            var stationStatusJson = await File.ReadAllTextAsync("TestData/BikeStationsStatus.json");
            var dto = JsonConvert.DeserializeObject<StationStatusDTO>(stationStatusJson);
            return dto.StationsStatusData.StationsStatus;
        }

        public async Task<SystemInformation> GetSystemInformationAsync()
        {
            return await Task.FromResult(new SystemInformation("1", "Test Bike Provider", "Nb", "Test Bike Provider inc.", "GMT", "44444", "SomeMail"));
        }
    }
}
