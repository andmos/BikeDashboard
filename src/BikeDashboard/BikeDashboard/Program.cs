﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;


namespace BikeDashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).UseLightInject()
                .Build()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseLightInject()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://::5000");
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
                    {
                        loggerConfiguration
                            .MinimumLevel.Information()
                            .ReadFrom.Configuration(hostingContext.Configuration)
                            .Enrich.FromLogContext();

                        if (hostingContext.HostingEnvironment.IsDevelopment())
                        {
                            loggerConfiguration.MinimumLevel.Verbose().WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
                        }
                        else
                        {
                            loggerConfiguration.MinimumLevel.Information().WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
                        }

                    });
                });
    }
}
