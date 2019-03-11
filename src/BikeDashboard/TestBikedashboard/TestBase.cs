using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using BikeDashboard;
using BikeDashboard.Services;
using BikeshareClient;
using Microsoft.Extensions.DependencyInjection;
using TestBikedashboard.Stubs;
using BikeDashboard.HealthChecks;

namespace TestBikedashboard
{
     public class TestBase
    {
        public string DefaultStation => "Ilaparken";

        public TestBase()
        {
            Factory = new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(builder => {
                builder.ConfigureTestServices(services => {
                ConfigureTestServices(services);
                });
            });
        }

        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
            IBikeshareClient bikeClient = new TestableBikeshareClient();
            IWeatherService weatherService = new TestableWeatherService();
            services.AddSingleton(bikeClient);
            services.AddSingleton(weatherService);
            services.AddSingleton<IStationService>(new StationService(bikeClient, DefaultStation));
            services.AddHealthChecks().AddCheck<BikeshareClientHealthCheck>("testBikeClient");
            services.AddHealthChecks().AddCheck<WeatherServiceHealthCheck>("testWeatherService");

        }


        public WebApplicationFactory<Startup> Factory { get; }
    }
}