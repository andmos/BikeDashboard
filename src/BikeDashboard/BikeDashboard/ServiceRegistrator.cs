using System;
using System.Net.Http;
using BikeDashboard.Configuration;
using BikeDashboard.HealthChecks;
using BikeDashboard.Services;
using BikeshareClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BikeDashboard
{
    public static class ServiceRegistrator
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            services.Configure<GbfsConfigurationSettings>(configuration);
            services.Configure<StationServiceSettings>(configuration);
            services.Configure<WeatherServiceSettings>(configuration);

            var gbfsAddress = configuration.Get<GbfsConfigurationSettings>().GBFSAddress;

            services.AddHttpClient("GBFSClient", httpClient =>
            {
                httpClient.BaseAddress = new Uri(gbfsAddress);
            });
            
            services.AddTransient<IBikeshareClient>(provider =>
            {
                var clientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = clientFactory.CreateClient("GBFSClient");
                return new Client("", httpClient);
            });
            
            services.AddTransient<IWeatherService, WeatherService>();
            services.Decorate<IWeatherService, TimeCachedWeatherService>();
            
            services.AddTransient<IStationService, StationService>();
        }

        public static void RegisterHealthChecks(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks().AddCheck<BikeshareClientHealthCheck>("BikeClient");
            if (configuration.Get<WeatherServiceSettings>().FeatureEnabled)
            {
                services.AddHealthChecks().AddCheck<WeatherServiceHealthCheck>("WeatherService");
            }
        }
    }
}