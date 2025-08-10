using GeneralVokiCreationService.Api.contracts.results;
using GeneralVokiCreationService.Api.extensions;
using GeneralVokiCreationService.Application.draft_vokis.commands.results;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
 using Microsoft.AspNetCore.Mvc;
 using VokimiStorageKeysLib.general_voki.result_image;

 namespace GeneralVokiCreationService.Api.endpoints;

internal static class SpecificVokiResultsHandlers
{
    internal static void MapSpecificVokiResultsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/results/{resultId}/");

        group.WithGroupAuthenticationRequired();

        group.MapGet("/", GetVokiResult);
        group.MapPut("/update", UpdateVokiResult)
            .WithRequestValidation<UpdateVokiResultRequest>();
        group.MapPost("/upload-image", UploadResultImage)
            .DisableAntiforgery();

        group.MapDelete("/delete", DeleteVokiResult);
    }

    private static async Task<IResult> GetVokiResult(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiResultQuery, VokiResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();

        GetVokiResultQuery query = new(vokiId, resultId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (vokiResult) => Results.Json(
            VokiResultDataResponse.Create(vokiResult)
        ));
    }

    private static async Task<IResult> UpdateVokiResult(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<UpdateVokiResultCommand, VokiResult> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateVokiResultRequest>();

        UpdateVokiResultCommand command = new(id, resultId,
            request.ParsedName, request.ParsedText, request.ParsedImage
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (voki) =>
            Results.Json(VokiResultDataResponse.Create(voki))
        );
    }

    private static async Task<IResult> UploadResultImage(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<UploadResultImageCommand, GeneralVokiResultImageKey> handler,
        [FromForm] IFormFile file
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();

        UploadResultImageCommand command = new(vokiId, resultId,
            new(file.OpenReadStream(), file.FileName, file.ContentType));
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (img) => Results.Json(
            new { ImageKey = img.ToString() }
        ));
    }
    private static async Task<IResult> DeleteVokiResult(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<DeleteVokiResultCommand> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();

        DeleteVokiResultCommand command = new(id, resultId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, CustomResults.Deleted);
    }
}