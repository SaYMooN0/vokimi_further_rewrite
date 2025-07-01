using GeneralVokiCreationService.Api.extensions;
using GeneralVokiCreationService.Application;
using GeneralVokiCreationService.Infrastructure;
using InfrastructureShared;

namespace GeneralVokiCreationService.Api;

public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.ConfigureLogging();
        
        builder.Services
            .AddPresentation(builder.Configuration)
            .AddApplication()
            .AddInfrastructure(builder.Configuration)
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