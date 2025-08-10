using GeneralVokiTakingService.Api.extensions;
using GeneralVokiTakingService.Application;
using GeneralVokiTakingService.Infrastructure;
using GeneralVokiTakingService.Infrastructure.persistence;
using InfrastructureShared;

namespace GeneralVokiTakingService.Api;


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
            .AddPresentation(builder.Configuration)
            .AddApplication()
            .AddInfrastructure(builder.Configuration, builder.Environment);
        ;

        var app = builder.Build();
        app.AddInfrastructureMiddleware();

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }
        else {
            app.UseHttpsRedirection();
        }

        app.AddExceptionHandlingMiddleware();

        app.MapEndpoints();

        app.AllowFrontendCors();
        app.Run();
    }
}