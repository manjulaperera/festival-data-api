using FestivalDataApi.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace FestivalDataApi
{
    /// <summary>
    /// Application entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application startup method
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            Log.Logger = new LoggerConfiguration()
                         .Configure(envName)
                         .CreateLogger();

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Create and configure the host with required services
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseSerilog((context, loggerConfiguration) =>
					{
						loggerConfiguration
							.Configure(context.HostingEnvironment.EnvironmentName)
							.ReadFrom.Configuration(context.Configuration);
					});
				});
    }
}
