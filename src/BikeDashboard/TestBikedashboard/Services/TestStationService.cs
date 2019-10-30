﻿using System;
using System.Threading.Tasks;
using Xunit;
using BikeDashboard.Services;
using BikeshareClient;
using System.Linq;
using TestBikedashboard.Stubs;
using Moq;
using BikeshareClient.Models;
using System.Collections.Generic;
using BikeDashboard.Models;
using Microsoft.Extensions.Options;
using BikeDashboard.Configuration;

namespace TestBikedashboard.Services
{
    public class TestStationService
    {

        private readonly string _defaultFavoriteStation;
        private readonly IBikeshareClient _bikeshareClientStub;
        private readonly IOptions<StationServiceSettings> _stationOptions;

        public TestStationService()
        {
            _defaultFavoriteStation = "Ilevollen";
            _stationOptions = Options.Create(new StationServiceSettings { StationName = _defaultFavoriteStation });
            _bikeshareClientStub = new TestableBikeshareClient();
        }

        [Fact]
        public async Task GetFavoriteStation_GivenValidDefaultStationName_ReturnFavoriteStation()
        {
            var cut = new StationService(_bikeshareClientStub, _stationOptions);

            var station = await cut.GetFavoriteStation();

            Assert.Equal(_defaultFavoriteStation, station.Name);
        }

        [Fact]
        public async Task GetFavoriteStation_GivenInvalidDefaultStationName_ThrowsArgumentException() 
        {
            _stationOptions.Value.StationName = "InvalidStation";
            var cut = new StationService(_bikeshareClientStub, _stationOptions);

            await Assert.ThrowsAsync<ArgumentException>(() => cut.GetFavoriteStation());
        }

        [Fact]
        public async Task GetFavoriteStation_GivenValidStationNameParameter_ReturnsFavoriteStation()
        {
            var cut = new StationService(_bikeshareClientStub, _stationOptions);
            var expectedStation = "Strandveikaia";

            var station = await cut.GetFavoriteStation(expectedStation);

            Assert.Equal(expectedStation, station.Name);
        }

        [Fact]
        public async Task GetFavoriteStation_GivenInvalidStationNameParameter_ReturnsDefaultFavoriteStation() 
        {
            var cut = new StationService(_bikeshareClientStub, _stationOptions);
            var expectedStation = _defaultFavoriteStation;

            var station = await cut.GetFavoriteStation("InvalidStation");

            Assert.Equal(expectedStation, station.Name);
        }

        [Fact]
        public async Task GetAvailableStations_ReturnsAllAvialableStation()
        {
            var cut = new StationService(_bikeshareClientStub, _stationOptions);

            var stations = await cut.GetAllAvailableStations();

            Assert.Equal(50, stations.Count());
        }

        [Fact]
        public async Task GetClosestAvailableStation_GivenValidStation_ReturnsClosestStationWithRentingBikes()
        {
            var cut = new StationService(_bikeshareClientStub, _stationOptions);
            var favoriteStation = await cut.GetFavoriteStation("Skansen");
            var expectedStationName = "Ilaparken";

            var closestStation = await cut.GetClosestAvailableStation(favoriteStation);

            Assert.Equal(expectedStationName, closestStation.Name);
        }

        [Fact]
        public async Task GetClosestAvailableStaiton_GivenSystemWithSingleEmptyStation_ReturnsNoAvailableStation() 
        {
            var emptyStationIdentity = new StationIdentity("mockStation", "mockStation");
            _stationOptions.Value.StationName = emptyStationIdentity.Name;
            var singleEmptyStationStatus = new List<StationStatus> { new StationStatus(emptyStationIdentity.Name, 0, 0, 1, true, true, true, 0, DateTime.Now) };
            var singleEmptyStation = new List<Station> { new Station(emptyStationIdentity.Id, emptyStationIdentity.Name, "Empty Address", 12, 12, 1) }; 
            var singleStationSystemMock = new Mock<IBikeshareClient>();
            singleStationSystemMock.Setup(s => s.GetStationsStatusAsync()).Returns(Task.FromResult<IEnumerable<StationStatus>>(singleEmptyStationStatus));
            singleStationSystemMock.Setup(s => s.GetStationsAsync()).Returns(Task.FromResult<IEnumerable<Station>>(singleEmptyStation));
            var cut = new StationService(singleStationSystemMock.Object, _stationOptions);

            var emptyStation = await cut.GetClosestAvailableStation(await cut.GetFavoriteStation());

            Assert.Equal("No Available Bikes found from bikeshare provider.", emptyStation.Name);

        }
    }
}