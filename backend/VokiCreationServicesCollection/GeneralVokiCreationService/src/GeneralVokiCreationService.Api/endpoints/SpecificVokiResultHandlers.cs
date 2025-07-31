using GeneralVokiCreationService.Api.contracts.results;
using GeneralVokiCreationService.Api.extensions;
using GeneralVokiCreationService.Application.draft_vokis.commands.results;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using Microsoft.AspNetCore.Mvc;
using VokimiStorageKeysLib.draft_general_voki.result_image;

namespace GeneralVokiCreationService.Api.endpoints;

internal static class SpecificVokiResultsHandlers
{
    internal static void MapSpecificVokiResultsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/results/{resultId}/");

        group.WithGroupAuthenticationRequired();

        group.MapGet("/", GetVokiResult);
        group.MapPatch("/update-name", UpdateResultName)
            .WithRequestValidation<UpdateResultNameRequest>();
        group.MapPatch("/update-text", UpdateResultText)
            .WithRequestValidation<UpdateResultTextRequest>();
        group.MapPatch("/remove-image", RemoveResultImage);
        group.MapPatch("/set-image", SetResultImage)
            .DisableAntiforgery();

        // group.MapDelete("/delete", DeleteVokiResult);
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

    private static async Task<IResult> UpdateResultName(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<UpdateResultNameCommand, VokiResultName> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateResultNameRequest>();

        UpdateResultNameCommand command = new(id, resultId, request.ParsedName);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (text) => Results.Json(
            new { NewName = text.ToString() }
        ));
    }

    private static async Task<IResult> UpdateResultText(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<UpdateResultTextCommand, VokiResultText> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateResultTextRequest>();

        UpdateResultTextCommand command = new(id, resultId, request.ParsedText);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (text) => Results.Json(
            new { NewText = text.ToString() }
        ));
    }

    private static async Task<IResult> SetResultImage(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<SetResultImageCommand, DraftGeneralVokiResultImageKey> handler,
        [FromForm] IFormFile file
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();

        SetResultImageCommand command = new(vokiId, resultId,
            new(file.OpenReadStream(), file.FileName, file.ContentType));
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (img) => Results.Json(
            new { NewImage = img.ToString() }
        ));
    }

    private static async Task<IResult> RemoveResultImage(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<RemoveResultImageCommand> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();

        RemoveResultImageCommand command = new(vokiId, resultId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }
}