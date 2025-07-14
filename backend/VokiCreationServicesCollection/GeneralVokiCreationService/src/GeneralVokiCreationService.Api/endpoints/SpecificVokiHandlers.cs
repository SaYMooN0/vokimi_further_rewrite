using GeneralVokiCreationService.Api.contracts.main_info;
using GeneralVokiCreationService.Application.draft_vokis.commands;
using GeneralVokiCreationService.Application.draft_vokis.commands.main_info;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel.common.vokis;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Api.endpoints;

public static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        group.WithGroupAuthenticationRequired();

        group.MapGet("/main-info", GetVokiMainInfo);

        group.MapPatch("/set-cover-to-default", SetVokiCoverToDefault);
        group.MapPatch("/update-cover", UpdateVokiCover)
            .WithRequestValidation<UpdateVokiCoverRequest>();

        group.MapPatch("/update-name", UpdateVokiName);
        group.MapPatch("/update-details", UpdateVokiDetails)
            .WithRequestValidation<UpdateVokiDetailsRequest>();
        group.MapPatch("/update-tags", UpdateVokiTags)
            .WithRequestValidation<UpdateVokiTagsRequest>();
    }

    private static async Task<IResult> GetVokiMainInfo(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiQuery, DraftGeneralVoki> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        GetVokiQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (voki) => Results.Json(
            VokiMainInfoResponse.Create(voki)
        ));
    }

    private static async Task<IResult> UpdateVokiName(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiNameCommand, VokiName> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateVokiNameRequest>();

        UpdateVokiNameCommand command = new(id, request.ParsedVokiName);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (name) => Results.Json(
            new { NewVokiName = name.ToString() }
        ));
    }

    private static async Task<IResult> SetVokiCoverToDefault(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<SetVokiCoverToDefaultCommand, DraftVokiCoverKey> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        SetVokiCoverToDefaultCommand command = new(id);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (key) => Results.Json(
            new { NewVokiCover = key.ToString() }
        ));
    }

    private static async Task<IResult> UpdateVokiCover(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiCoverCommand, DraftVokiCoverKey> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateVokiCoverRequest>();

        UpdateVokiCoverCommand command = new(id, request.ParsedCoverKey);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (key) => Results.Json(
            new { NewVokiCover = key.ToString() }
        ));
    }

    private static async Task<IResult> UpdateVokiDetails(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiDetailsCommand, VokiDetails> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateVokiDetailsRequest>();

        UpdateVokiDetailsCommand command = new(id, request.ParsedDetails);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (details => Results.Json(details)));
    }

    private static async Task<IResult> UpdateVokiTags(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiTagsCommand, VokiTagsSet> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateVokiTagsRequest>();

        UpdateVokiTagsCommand command = new(id, request.ParsedTags);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (tagsSet) => Results.Json(
            new { Tags = tagsSet.Value.Select(t=>t.ToString()).ToArray() }
        ));
    }
}