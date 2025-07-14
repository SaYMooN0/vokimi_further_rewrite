using InfrastructureShared;
using TagsService.Api.extensions;
using TagsService.Application;
using TagsService.Infrastructure;

namespace TagsService.Api;

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
            // var db = serviceScope.ServiceProvider.GetRequiredService<TagsDbContext>();
            // db.Database.EnsureDeleted();
            // db.Database.EnsureCreated();
        }

        app.AllowFrontendCors();
        app.Run();
    }
}