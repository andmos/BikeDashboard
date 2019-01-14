using BikeshareClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BikeDashboard.Services;
using BikeDashboard.HealthChecks;

namespace BikeDashboard
{
    public class Startup
    {
		public Startup(IHostingEnvironment env)
        {
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables();

			Configuration = builder.Build();
        }

		public void ConfigureServices(IServiceCollection services)
        {
			services.AddMvc();
			// Add functionality to inject IOptions<T>
            services.AddOptions();

            var gbfsAddress = Configuration.GetValue<string>("GBFSAddress");
            var bikeClient = new Client(gbfsAddress);
			var weatherService = new WeatherService(Configuration.GetValue<string>("WeatherServiceAPIKey"));
			services.AddSingleton<IBikeshareClient>(bikeClient);
			services.AddSingleton<IWeatherService>(weatherService);
			services.AddSingleton<IStationService>(new StationService(bikeClient, Configuration.GetValue<string>("StationName")));

            services.AddHealthChecks().AddCheck<BikeshareClientHealthCheck>(gbfsAddress);
        }

        public IConfiguration Configuration { get; }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseHealthChecks("/api/health");
            app.UseMvc(routes =>
            {
			routes.MapRoute(
				name: "api",
				template: "api/{controller=FavoriteStation}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
