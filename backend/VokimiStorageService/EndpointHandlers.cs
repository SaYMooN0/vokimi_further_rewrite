using ApiShared;
using VokimiStorageService.buckets;

namespace VokimiStorageService;

internal static class EndpointHandlers
{
    internal static void MapEndpointHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/");
        
        group
            .DisableAntiforgery()
            .MapGet("/main/{*fileKey}", GetFileFromStorage);
    }

    private static async Task<IResult> GetFileFromStorage(
        CancellationToken ct,
        string fileKey,
        MainStorageBucket bucketAccessor
    ) {
        var result = await bucketAccessor.GetFileAsync(fileKey);
        return CustomResults.FromErrOr(result, (obj) => Results.Stream(
                stream: obj.Stream,
                contentType: obj.ContentType,
                fileDownloadName: fileKey
            )
        );
    }
}