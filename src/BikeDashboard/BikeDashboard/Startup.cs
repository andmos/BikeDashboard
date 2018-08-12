using BikeshareClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BikeDashboard.Services;

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

			var bikeClient = new Client(Configuration.GetValue<string>("GBFSAddress"));
			services.AddSingleton<IBikeshareClient>(bikeClient);
			services.AddSingleton<IStationService>(new StationService(bikeClient, Configuration.GetValue<string>("StationName")));
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
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
