using ApiShared;
using ApiShared.extensions;
using SharedKernel.errs;
using VokimiStorageKeysLib.draft_voki_cover;
using VokimiStorageService.contracts;
using VokimiStorageService.storage_service.buckets;

namespace VokimiStorageService.endpoints;

public static class MainBucketHandlers
{
    internal static void MapMainBucketHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/");

        group.DisableAntiforgery();

        group.MapGet("/main/files/{*fileKey}", GetFileFromStorage);
        group.MapPost("/main/upload/draftVokiCover", UploadDraftVokiCoverToStorage)
            .WithRequestValidation<RequestWithVokiId>();
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

    private static async Task<IResult> UploadDraftVokiCoverToStorage(
        HttpContext httpContext,
        CancellationToken ct,
        MainStorageBucket bucket,
        IFormFile file
    ) {
        // validate for max file size

        var request = httpContext.GetValidatedRequest<RequestWithVokiId>();

        string extenstion = Path.GetExtension(file.FileName);
        DraftVokiCoverKey coverKey = DraftVokiCoverKey.CreateWithId(request.ParsedVokiId(), extenstion);

        var result = await bucket.UploadFileAsync(coverKey, file.OpenReadStream(), file.ContentType);
        return CustomResults.FromErrOrNothing(result,
            () => CustomResults.WithFileKey(coverKey.ToString())
        );
    }
}