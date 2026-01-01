using System.Reflection;
using AlbumsService.Application;
using AlbumsService.Infrastructure;
using ApiShared;
using ApiShared.extensions;
using InfrastructureShared.Base;

namespace AlbumsService.Api;


public class Program
{
    public static void Main(string[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        
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
        app.MapEndpointGroups();
        app.AllowFrontendCors();

        app.Run();
    }
}