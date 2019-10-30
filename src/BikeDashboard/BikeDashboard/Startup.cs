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
using BikeDashboard.Extensions;
using BikeDashboard.Configuration;
using System.Net.Http;

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

            services.AddMemoryCache();
            services.AddHttpClient();

            services.Configure<StationServiceSettings>(Configuration);
            services.Configure<WeatherServiceSettings>(Configuration);

            var gbfsAddress = Configuration.GetValue<string>("GBFSAddress");

            services.AddHttpClient("GBFSClient", client =>
            {
                client.BaseAddress = new Uri(gbfsAddress);
            });

            services.AddTransient<IBikeshareClient, Client>(sp =>
                new Client("", sp.GetService<IHttpClientFactory>().CreateClient("GBFSClient")));
            
            services.AddTransient<IWeatherService, WeatherService>();
            services.Decorate<IWeatherService, TimeCachedWeatherService>();
            services.AddTransient<IStationService, StationService>();

            services.AddHealthChecks().AddCheck<BikeshareClientHealthCheck>("BikeClient");
            var provider = services.BuildServiceProvider();
            var registeredWeatherService = provider.GetService<IWeatherService>();
            if (registeredWeatherService.FeatureEnabled)
            {
                services.AddHealthChecks().AddCheck<WeatherServiceHealthCheck>("WeatherService");
            }
            services.BuildServiceProvider();
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
