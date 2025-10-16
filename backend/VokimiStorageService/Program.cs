using ApiShared;
using Infrastructure.Auth;
using VokimiStorageService.extensions;

namespace VokimiStorageService;

public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseDefaultServiceProvider((_, options) => {
            options.ValidateScopes = false;
            options.ValidateOnBuild = true;
        });
        builder.ConfigureLogging();

        builder.Services
            .AddAuth(builder.Configuration)
            .AddWeb(builder.Configuration)
            .AddS3Storage(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }
        else {
            app.UseHttpsRedirection();
        }

        app.AddExceptionHandlingMiddleware();

        app.MapEndpointHandlers();

        app.Run();
    }
}