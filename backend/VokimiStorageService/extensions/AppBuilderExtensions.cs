using Serilog;
using Serilog.Events;

namespace VokimiStorageService.extensions;

public static class AppBuilderExtensions
{
    public static void ConfigureLogging(this WebApplicationBuilder builder) {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "VokimiBackend")
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .WriteTo.Console()
            .CreateLogger();
            
        builder.Host.UseSerilog();
    }
}