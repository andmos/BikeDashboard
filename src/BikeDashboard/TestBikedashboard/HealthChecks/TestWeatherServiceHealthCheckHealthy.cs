using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestBikedashboard.DTO;
using Xunit;

namespace TestBikedashboard.HealthChecks
{
    public class TestWeatherServiceHealthCheckHealthy : TestBase
    {
        [Fact]
        public async Task CheckHealthAsync_GivenValidWeatherService_ReturnsHealthy()
        {
            var response = await Factory.CreateClient().GetAsync("/api/health");

            var content = await response.Content.ReadAsStringAsync();
            var healthCheckStatus = JsonConvert
            .DeserializeObject<HealthCheckDTO>(content)
                .Checks.FirstOrDefault(check => check.Service.Equals("testWeatherService")).Status;

            response.EnsureSuccessStatusCode();
            Assert.Equal("Healthy", healthCheckStatus);
        }
    }
}
