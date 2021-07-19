using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Json;

namespace FestivalDataApi.Utilities
{
    /// <summary>
    /// This class defines the logger configurations and the file name, location and how to store it etc
    /// </summary>
    public static class LoggerConfigurationExtensions
    {
        /// <summary>
        /// Configure the logger with given configurations
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="envName"></param>
        /// <returns></returns>
        public static LoggerConfiguration Configure(this LoggerConfiguration configuration, string envName)
        {
            var app = "Manjula.FestivalDataApi";
            var filename = $"C:\\Logs\\{envName}\\{app}\\{app}-";
            var template = "{Timestamp:yyyy:MM:dd HH:mm:ss.fff} [{Level:4}] [{RequestId}] {Message}{NewLine}{Exception}";

            return configuration
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Information()
                .Filter.ByExcluding(e => e.Properties.TryGetValue("RequestPath", out var requestPath)
                                         && requestPath.ToString().ToLower().Contains("/diagnostics/"))
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", app)
                .Enrich.WithProperty("Environment", envName)
                .Enrich.WithExceptionDetails()
#if DEBUG
                .WriteTo.File($"{filename}.txt", outputTemplate: template, fileSizeLimitBytes:null)
#endif
                .WriteTo.File(new JsonFormatter(), $"{filename}.log", rollingInterval: RollingInterval.Day ,rollOnFileSizeLimit: true, retainedFileCountLimit: 5, fileSizeLimitBytes:
                    10485760);
        }
    }
}
