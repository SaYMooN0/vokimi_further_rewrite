using ApiShared;
using VokimiStorageService.buckets;
using VokimiStorageService.extensions;

namespace VokimiStorageService;

public class Program
{
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.ConfigureLogging();

        builder.Services.AddS3Storage(builder.Configuration);

        var app = builder.Build();

        if (app.Environment.IsDevelopment()) {
            app.MapOpenApi();
        }
        else {
            app.UseHttpsRedirection();
        }

        app.AddExceptionHandlingMiddleware();

        app
            .MapGroup("/")
            .DisableAntiforgery()
            .MapGet("/main/{*fileKey}", GetFileFromStorage);

        app.Run();
    }
    private static async Task<IResult> GetFileFromStorage(
        CancellationToken ct,
        string fileKey,
        MainStorageBucket bucket
    ) {
        var result = await bucket.GetFileAsync(fileKey);
        return CustomResults.FromErrOr(result, (obj) => Results.Stream(
                stream: obj.Stream,
                contentType: obj.ContentType,
                fileDownloadName: fileKey
            )
        );
    }
}