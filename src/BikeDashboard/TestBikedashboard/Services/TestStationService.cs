using System;
using System.Threading.Tasks;
using Xunit;
using BikeDashboard.Services;
using BikeshareClient;
using System.Linq;
using TestBikedashboard.Stubs;

namespace TestBikedashboard.Services
{
    public class TestStationService
    {

        private readonly string _defaultFavoriteStation;
        private readonly IBikeshareClient _bikeshareClientStub;

        public TestStationService()
        {
            _defaultFavoriteStation = "Ilevollen";
            _bikeshareClientStub = new TestableBikeshareClient();
        }

        [Fact]
        public async Task GetFavoriteStation_GivenValidDefaultStationName_ReturnFavoriteStation()
        {
            var cut = new StationService(_bikeshareClientStub, _defaultFavoriteStation);

            var station = await cut.GetFavoriteStation();

            Assert.Equal(_defaultFavoriteStation, station.Name);
        }

        [Fact]
        public async Task GetFavoriteStation_GivenInvalidDefaultStationName_ThrowsArgumentException() 
        {
            var cut = new StationService(_bikeshareClientStub, "InvalidStation");

            await Assert.ThrowsAsync<ArgumentException>(() => cut.GetFavoriteStation());
        }

        [Fact]
        public async Task GetFavoriteStation_GivenValidStationNameParameter_ReturnsFavoriteStation()
        {
            var cut = new StationService(_bikeshareClientStub, _defaultFavoriteStation);
            var expectedStation = "Strandveikaia";

            var station = await cut.GetFavoriteStation(expectedStation);

            Assert.Equal(expectedStation, station.Name);
        }

        [Fact]
        public async Task GetFavoriteStation_GivenInvalidStationNameParameter_ReturnsDefaultFavoriteStation() 
        {
            var cut = new StationService(_bikeshareClientStub, _defaultFavoriteStation);
            var expectedStation = _defaultFavoriteStation;

            var station = await cut.GetFavoriteStation("InvalidStation");

            Assert.Equal(expectedStation, station.Name);
        }

        [Fact]
        public async Task GetAvailableStations_ReturnsAllAvialableStation()
        {
            var cut = new StationService(_bikeshareClientStub, _defaultFavoriteStation);

            var stations = await cut.GetAllAvailableStations();

            Assert.Equal(50, stations.Count());
        }

        [Fact]
        public async Task GetClosestAvailableStation_GivenValidStation_ReturnsClosestStationWithRentingBikes()
        {
            var cut = new StationService(_bikeshareClientStub, _defaultFavoriteStation);
            var favoriteStation = await cut.GetFavoriteStation("Skansen");
            var expectedStationName = "Ilaparken";

            var closestStation = await cut.GetClosestAvailableStation(favoriteStation);

            Assert.Equal(expectedStationName, closestStation.Name);
        }
    }
}
