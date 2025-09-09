using AlbumsService.Api.extensions;
using AlbumsService.Application;
using AlbumsService.Infrastructure;
using ApiShared;
using InfrastructureShared.Base;

namespace AlbumsService.Api;


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