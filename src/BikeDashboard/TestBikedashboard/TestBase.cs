using System;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Xunit.Abstractions;
using BikeDashboard;
using TestBikedashboard.Helpers;
using BikeDashboard.Services;
using BikeshareClient;
using Microsoft.Extensions.DependencyInjection;

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
            IWeatherService weatherService = new WeatherService("");
            services.AddSingleton(bikeClient);
            services.AddSingleton(weatherService);
            services.AddSingleton<IStationService>(new StationService(bikeClient, DefaultStation));
        }


        public WebApplicationFactory<Startup> Factory { get; }
    }
}