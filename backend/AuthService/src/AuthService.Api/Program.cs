using AuthService.Api.extensions;
using AuthService.Application;
using AuthService.Infrastructure;
using AuthService.Infrastructure.persistence;
using InfrastructureShared;

namespace AuthService.Api;

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
        
        using (var serviceScope = app.Services.CreateScope()) {
            var db = serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>();
            // db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        app.AllowFrontendCors();
        app.Run();
    }
}