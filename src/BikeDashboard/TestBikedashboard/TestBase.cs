using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using BikeDashboard;
using BikeDashboard.Services;
using BikeshareClient;
using Microsoft.Extensions.DependencyInjection;
using TestBikedashboard.Stubs;
using BikeDashboard.HealthChecks;
using BikeDashboard.Configuration;
using Microsoft.Extensions.Configuration;

namespace TestBikedashboard
{
     public class TestBase
    {
        public string DefaultStation => "Ilaparken";

        public TestBase()
        {
            Factory = new WebApplicationFactory<Startup>()
            .WithWebHostBuilder(builder => {
                Configuration = new ConfigurationBuilder().Build();
                builder.ConfigureTestServices(services => {
                    services.AddOptions();  
                ConfigureTestServices(services);
                });
            });
        }

        public IConfigurationRoot Configuration { get; private set; }

        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
            services.Configure<StationServiceSettings>(Configuration);

            IBikeshareClient bikeClient = new TestableBikeshareClient();
            IWeatherService weatherService = new TestableWeatherService();

            services.AddSingleton(bikeClient);
            services.AddSingleton(weatherService);
            services.AddSingleton<IStationService, StationService>();
            services.AddHealthChecks().AddCheck<BikeshareClientHealthCheck>("testBikeClient");
            services.AddHealthChecks().AddCheck<WeatherServiceHealthCheck>("testWeatherService");

        }

        public WebApplicationFactory<Startup> Factory { get; }
    }
}