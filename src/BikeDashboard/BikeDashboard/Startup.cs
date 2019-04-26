using BikeshareClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BikeDashboard.Services;
using BikeDashboard.HealthChecks;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.HttpOverrides;

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
            Console.WriteLine(env.WebRootPath);
            Configuration = builder.Build();
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // Add functionality to inject IOptions<T>
            services.AddOptions();

            var gbfsAddress = Configuration.GetValue<string>("GBFSAddress");
            IBikeshareClient bikeClient = new Client(gbfsAddress);
            IWeatherService weatherService = new WeatherService(Configuration.GetValue<string>("WeatherServiceAPIKey"));
            services.AddSingleton(bikeClient);
            services.AddSingleton(weatherService);
            services.AddSingleton<IStationService>(new StationService(bikeClient, Configuration.GetValue<string>("StationName")));

            services.AddHealthChecks().AddCheck<BikeshareClientHealthCheck>(nameof(bikeClient));
            if (weatherService.FeatureEnabled)
            {
                services.AddHealthChecks().AddCheck<WeatherServiceHealthCheck>(nameof(weatherService));
            }
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
            app.UsePathBase(Configuration["PathBase"]);

            var healthCheckOptions = CreateHealthCheckOptions();

            app.UseHealthChecks("/api/health", healthCheckOptions);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "api",
                    template: "api/{controller=FavoriteStation}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });


        }

        private HealthCheckOptions CreateHealthCheckOptions()
        {
            var options = new HealthCheckOptions();
            options.ResultStatusCodes[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable;
            options.ResultStatusCodes[HealthStatus.Degraded] = StatusCodes.Status206PartialContent;

            options.ResponseWriter = async (ctx, rpt) =>
            {
                var result = JsonConvert.SerializeObject(new
                {
                    status = rpt.Status.ToString(),
                    checks = rpt.Entries.Select(e => new { service = e.Key, status = Enum.GetName(typeof(HealthStatus), e.Value.Status), error = e.Value.Exception?.Message })
                },
                Formatting.None, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                ctx.Response.ContentType = MediaTypeNames.Application.Json;
                await ctx.Response.WriteAsync(result);
            };
            return options;
        }
    }
}
