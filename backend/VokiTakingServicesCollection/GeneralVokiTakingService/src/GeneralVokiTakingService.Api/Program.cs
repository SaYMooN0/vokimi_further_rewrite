using System.Reflection;
using GeneralVokiTakingService.Application;
using GeneralVokiTakingService.Infrastructure;
using InfrastructureShared.Base;

namespace GeneralVokiTakingService.Api;


public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseDefaultServiceProvider((_, options) => {
            options.ValidateScopes = false;
            options.ValidateOnBuild = true;
        });
        builder.Services.AddConfiguredLogging(builder.Configuration);
        builder.Services
            .AddApplication()
            .AddInfrastructure(builder.Configuration, builder.Environment)
            .AddPresentation(builder.Configuration)
            .AddEndpoints(Assembly.GetExecutingAssembly())
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
        app.AllowFrontendCors();
        app.MapEndpointGroups();
        
        app.Run();
    }
}