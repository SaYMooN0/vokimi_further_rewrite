using ApiShared;
using ApiShared.extensions;
using ApplicationShared;
using Microsoft.AspNetCore.Mvc;
using VokimiStorageService.s3_storage.s3;
using VokimiStorageService.s3_storage.storage_service;

namespace VokimiStorageService;

internal class EndpointHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/main");

        group.MapGet("/{*fileKey}", GetFileFromStorage)
            .DisableAntiforgery();
        group.MapPut("/upload-temp-image", UploadTempImage)
            .DisableAntiforgery();
        
        return group;
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

    private static async Task<IResult> UploadTempImage(
        HttpContext httpContext,
        CancellationToken ct,
        [FromForm] IFormFile file,
        IStorageService storageService,
        IUserContext userContext
    ) {
        if (userContext.UserIdFromToken().IsErr()) {
            return CustomResults.ErrorResponse(ErrFactory.AuthRequired());
        }

        FileData fileData = new(file.OpenReadStream(), file.ContentType);
        ErrOr<TempImageKey> res = await storageService.PutTempImageFile(fileData, ct);

        return CustomResults.FromErrOr(res, (key) => Results.Json(
            new { TempKey = key.ToString() }
        ));
    }
}