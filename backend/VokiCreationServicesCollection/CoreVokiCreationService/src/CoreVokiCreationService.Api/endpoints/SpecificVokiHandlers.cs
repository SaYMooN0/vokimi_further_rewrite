using CoreVokiCreationService.Api.contracts.vokis_brief_info;
using CoreVokiCreationService.Application.draft_vokis.queries;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Api.endpoints;

public static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        group.WithGroupAuthenticationRequired();

        // group.MapPatch("/invite-co-author", InviteCoAuthor)
        //     .WithRequestValidation<InviteCoAuthorRequest>();
        group.MapGet("/brief-info", GetVokiBriefInfo);
        // group.MapGet("/view-data-for-co-author-invited", GetVokiViewDataForCoAuthorInvitedUser); 
    }

    private static async Task<IResult> GetVokiBriefInfo(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<GetVokiQuery, DraftVoki> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        GetVokiQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (voki) => Results.Json(
            VokiBriefInfoResponse.FromVoki(voki)
        ));
    }
}