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
    public class TestBikeshareClientHealthCheckFailure : TestBase
    {

        protected override void ConfigureTestServices(IServiceCollection services)
        {
             base.ConfigureTestServices(services);
             services.AddSingleton<IBikeshareClient>(new Client("InvalidUri"));
        }

         [Fact]
        public async Task CheckHealthAsync_GivenValidBikeshareClient_ReturnsDegraded()
        {
            var client = Factory.CreateClient();

            var response = await client.GetAsync("/api/health");

            var content = await response.Content.ReadAsStringAsync();
            var healthCheckStatus = JsonConvert
            .DeserializeObject<HealthCheckDTO>(content)
                .Checks.FirstOrDefault(check => check.Service.Equals("bikeClient")).Status;


            Assert.Equal("Unhealthy", healthCheckStatus);
        }
    }
}
