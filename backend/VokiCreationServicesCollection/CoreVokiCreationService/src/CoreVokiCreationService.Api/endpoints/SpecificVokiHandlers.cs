using CoreVokiCreationService.Api.contracts;
using CoreVokiCreationService.Api.contracts.vokis_brief_info;
using CoreVokiCreationService.Application.draft_vokis.commands;
using CoreVokiCreationService.Application.draft_vokis.queries;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Api.endpoints;

public static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        group.WithGroupAuthenticationRequired();

        group.MapGet("/brief-info", GetVokiBriefInfo);
        group.MapGet("/authors-info", GetVokiAuthorsInfo);
        group.MapPost("/invite-co-author", InviteCoAuthor)
            .WithRequestValidation<InviteCoAuthorRequest>();
        // group.MapGet("/view-data-for-co-author-invited", GetVokiViewDataForCoAuthorInvitedUser); 
        // group.MapDelete("/cancel-co-author-invite", CancelCoAuthorInvite);
        // group.MapPatch("/accept-co-author-invite", AcceptCoAuthorInvite);
        // group.MapPatch("/decline-co-author-invite", DeclineCoAuthorInvite);

    }

    private static async Task<IResult> GetVokiBriefInfo(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<GetVokiQuery, DraftVoki> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        GetVokiQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (voki) => Results.Json(
            VokiBriefInfoResponse.Create(voki)
        ));
    }

    private static async Task<IResult> GetVokiAuthorsInfo(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<GetVokiQuery, DraftVoki> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        GetVokiQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (voki) => Results.Json(
            VokiAuthorsInfoResponse.Create(voki)
        ));
    }

    private static async Task<IResult> InviteCoAuthor(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<InviteCoAuthorCommand, DraftVoki> handler
    ) {
        var request = httpContext.GetValidatedRequest<InviteCoAuthorRequest>();
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        InviteCoAuthorCommand command = new(vokiId, request.ParsedNewCoAuthorId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (voki) => Results.Json(
            VokiAuthorsInfoResponse.Create(voki)
        ));
    }
}