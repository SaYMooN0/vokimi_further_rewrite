using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;

namespace InfrastructureShared;

public static class AppBuilderExtensions
{
    public static void ConfigureLogging(this WebApplicationBuilder builder) {
        var serviceName = builder.Configuration["ServiceName"]
                          ?? throw new ArgumentNullException("ServiceName is not provided");

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