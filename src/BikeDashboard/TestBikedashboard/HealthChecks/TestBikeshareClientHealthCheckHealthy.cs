using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BikeDashboard;
using BikeshareClient;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.DependencyInjection;
using TestBikedashboard.DTO;
using Xunit;
using BikeDashboard.Services;

namespace TestBikedashboard.HealthChecks
{
    public class TestBikeshareClientHealthCheckHealthy : TestBase
    {


        [Fact]
        public async Task CheckHealthAsync_GivenValidBikeshareClient_ReturnsHealth()
        {

            var response = await Factory.CreateClient().GetAsync("/api/health");

            var content = await response.Content.ReadAsStringAsync();
            var healthCheckStatus = JsonConvert
            .DeserializeObject<HealthCheckDTO>(content)
                .Checks.FirstOrDefault(check => check.Service.Equals("bikeClient")).Status;

            response.EnsureSuccessStatusCode();
            Assert.Equal("Healthy", healthCheckStatus);
        }


    }
}
