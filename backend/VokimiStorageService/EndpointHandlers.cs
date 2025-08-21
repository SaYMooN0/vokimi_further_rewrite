using ApiShared;
using VokimiStorageService.s3_storage.s3;

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
        IS3MainBucketClient s3MainBucketClient
    ) {
        ErrOr<FileData> result = await s3MainBucketClient.GetFile(fileKey, ct);
        return CustomResults.FromErrOr(result, (file) => Results.Stream(
                stream: file.Stream,
                contentType: file.ContentType,
                fileDownloadName: fileKey
            )
        );
    }
}