using ApiShared;
using VokimiStorageService.extensions;

namespace VokimiStorageService;

public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.ConfigureLogging();

        builder.Services.AddUserContext();
        builder.Services.AddS3Storage(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }
        else {
            app.UseHttpsRedirection();
        }

        app.AddExceptionHandlingMiddleware();

        app.MapEndpoints();

        app.Run();
    }
}