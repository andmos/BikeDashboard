using System;
using System.Net.Http;
using System.Threading.Tasks;
using BikeDashboard.DTO;
using Newtonsoft.Json;
using Xunit;
using System.Linq;

namespace TestBikedashboard.Controllers
{
    public class TestFavoriteStationController : TestBase
    {
        private readonly HttpClient _client;

        public TestFavoriteStationController()
        {
            _client = Factory.CreateClient();
        }

        [Fact]
        public async Task Get_GivenCorrectConfiguration_ReturnsStationWeatherDTO() 
        {
            var response = await _client.GetAsync("/api/FavoriteStation");

            var content = await response.Content.ReadAsStringAsync();

            var station = JsonConvert.DeserializeObject<StationWeatherDTO>(content);

            Assert.Equal(DefaultStation, station.Station.Name);
        }

        [Fact]
        public async Task Get_GivenValidStationAsQueryString_ReturnsStationWeatherDTO()
        {
            var response = await _client.GetAsync("/api/FavoriteStation?stationName=skansen");

            var content = await response.Content.ReadAsStringAsync();

            var station = JsonConvert.DeserializeObject<StationWeatherDTO>(content);

            Assert.Equal("Skansen", station.Station.Name);
        }

        [Fact]
        public async Task GetAsync_GivenInvalidStationQuery_ReturnsDefaultStation()
        {
            var response = await _client.GetAsync("/api/FavoriteStation?stationName=skanseeeen");

            var content = await response.Content.ReadAsStringAsync();
            var station = JsonConvert.DeserializeObject<StationWeatherDTO>(content);

            Assert.Equal(DefaultStation, station.Station.Name);
        }
    }
}
