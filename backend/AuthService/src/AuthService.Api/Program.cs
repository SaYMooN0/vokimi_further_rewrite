using System.Reflection;
using ApiShared.EfCore;
using AuthService.Application;
using AuthService.Infrastructure;
using AuthService.Infrastructure.persistence;
using InfrastructureShared.Base;
using InfrastructureShared.EfCore;

namespace AuthService.Api;

public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseDefaultServiceProvider((_, options) => {
            options.ValidateScopes = false;
            options.ValidateOnBuild = true;
        });

        builder.Services.AddFrontendConfig(builder.Configuration);
        builder.Services.AddConfiguredLogging(builder.Configuration);

        builder.Services
            .AddApplication()
            .AddInfrastructure(builder.Configuration, builder.Environment)
            .AddPresentation(builder.Configuration)
            .AddEndpoints(Assembly.GetExecutingAssembly())
            ;

        var app = builder.Build();
        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }
        else {
            app.UseHttpsRedirection();
        }

        app.AddExceptionHandlingMiddleware();
        app.MapAllEndpointWithConsistency<AuthDbContext>();
        app.AllowFrontendCors();
        
        app.Run();
    }
}