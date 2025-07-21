using InfrastructureShared;
using UserProfilesService.Api.extensions;
using UserProfilesService.Application;
using UserProfilesService.Infrastructure;
using UserProfilesService.Infrastructure.persistence;

namespace UserProfilesService.Api;

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
            var db = serviceScope.ServiceProvider.GetRequiredService<UserProfilesDbContext>();
            // db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        app.AllowFrontendCors();
        app.Run();
    }
}