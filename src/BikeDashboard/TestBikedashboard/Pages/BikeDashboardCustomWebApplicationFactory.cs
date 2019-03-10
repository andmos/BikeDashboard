using System;
using BikeDashboard.Services;
using BikeshareClient;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TestBikedashboard.Helpers;

namespace TestBikedashboard.Pages
{
    public class BikeDashboardCustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {                    
                IBikeshareClient bikeClient = new TestableBikeshareClient();
                IWeatherService weatherService = new WeatherService("");
                services.AddSingleton(bikeClient);
                services.AddSingleton(weatherService);
                services.AddSingleton<IStationService>(new StationService(bikeClient, "Ilaparken"));

            });
        }
    }
}
