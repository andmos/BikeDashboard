using System;
using BikeDashboard.Services;
using Xunit;
using TestBikedashboard.Stubs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using BikeDashboard.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TestBikedashboard.Extensions
{
    public class TestTimeCachedWeatherService
    {
        [Fact]
        public async Task GetDailyForeCastAsync_GivenStationCoordinates_ReturnsWeatherForcast() 
        {
            var cut = CreateClassUnderTest();
            var testCoordinates = new StationCoordinates { Latitude = 100, Longitude = 200 };

            var weatherForcast = await cut.GetDailyForeCastAsync(testCoordinates);

            Assert.Equal(2.0, weatherForcast.Forecasts.FirstOrDefault().Rain.Value);
        }

        [Fact] 
        public async Task GetDailyForeCastAsync_GivenSameStationCoordinates_ReturnsSameInstance() 
        {
            var cut = CreateClassUnderTest();
            var testCoordinates = new StationCoordinates { Latitude = 100, Longitude = 200 };

            var weatherForcast1 = await cut.GetDailyForeCastAsync(testCoordinates);
            var weatherForcast2 = await cut.GetDailyForeCastAsync(testCoordinates);

            Assert.Same(weatherForcast1, weatherForcast2);
        }

        [Fact]
        public void FeatureEnabled_GivenValidIWeatherService_FeatureEnabled() 
        {
            var cut = CreateClassUnderTest();

            Assert.True(cut.FeatureEnabled);
        }

        private TimeCachedWeatherService CreateClassUnderTest() 
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            var weatherServiceStub = new TestableWeatherService();

            return new TimeCachedWeatherService(weatherServiceStub, memoryCache);
        }
    }
}
