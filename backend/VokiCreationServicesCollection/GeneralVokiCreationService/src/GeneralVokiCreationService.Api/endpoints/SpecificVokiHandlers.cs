using GeneralVokiCreationService.Api.contracts;
using GeneralVokiCreationService.Application.draft_vokis.commands;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Api.endpoints;

public static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        group.WithGroupAuthenticationRequired();

        group.MapGet("/main-info", GetVokiMainInfo);
        group.MapPatch("/update-name", UpdateVokiName);
        // group.MapPatch("/set-cover-to-default", SetVokiCoverToDefault);
        // group.MapPatch("/update-cover", UpdateVokiCover)
        //     .WithRequestValidation<UpdateVokiCoverRequest>();
        // group.MapPatch("/update-details", UpdateVokiDetails)
        //     .WithRequestValidation<UpdateVokiDetailsRequest>();
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

        return CustomResults.FromErrOr(result, (name) => CustomResults.Created(
            new { NewVokiName = name.ToString() }
        ));
    }
}