using System;
using System.Net.Http;
using System.Threading.Tasks;
using BikeDashboard;
using BikeDashboard.Models;
using Newtonsoft.Json;
using Xunit;

namespace TestBikedashboard.Controllers
{
    public class TestSystemStatusController : TestBase
    {
        private readonly HttpClient _client;

        public TestSystemStatusController()
        {
            _client = Factory.CreateClient();
        }

        [Fact]
        public async Task GetAsync_ReturnsSystemStatus()
        {
            var response = await _client.GetAsync("/api/SystemStatus");

            var content = await response.Content.ReadAsStringAsync();
            var systemStatus = JsonConvert.DeserializeObject<SystemStatus>(content);

            Assert.Equal("Test Bike Provider", systemStatus.SystemName);
        }
    }
}
