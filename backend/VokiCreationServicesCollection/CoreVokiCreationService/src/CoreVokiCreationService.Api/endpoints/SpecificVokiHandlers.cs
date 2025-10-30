using CoreVokiCreationService.Api.contracts;
using CoreVokiCreationService.Api.contracts.vokis_brief_info;
using CoreVokiCreationService.Application.draft_vokis.commands.invites;
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

        group.MapDelete("/drop-co-author", DropCoAuthor)
            .WithRequestValidation<VokiCoAuthorActionRequest>();
        group.MapPost("/invite-co-author", InviteCoAuthor)
            .WithRequestValidation<VokiCoAuthorActionRequest>();
        group.MapDelete("/cancel-co-author-invite", CancelCoAuthorInvite)
            .WithRequestValidation<VokiCoAuthorActionRequest>();

        group.MapPatch("/accept-co-author-invite", AcceptCoAuthorInvite);
        group.MapPatch("/decline-co-author-invite", DeclineCoAuthorInvite);
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
        var request = httpContext.GetValidatedRequest<VokiCoAuthorActionRequest>();
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        InviteCoAuthorCommand command = new(vokiId, request.ParsedUserId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<DraftVoki, VokiCoAuthorsWithInvitedResponse>(result);
    }
    private static async Task<IResult> DropCoAuthor(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<DropCoAuthorCommand, DraftVoki> handler
    ) {
        var request = httpContext.GetValidatedRequest<VokiCoAuthorActionRequest>();
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        DropCoAuthorCommand command = new(vokiId, request.ParsedUserId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<DraftVoki, VokiCoAuthorsWithInvitedResponse>(result);
    }

    private static async Task<IResult> CancelCoAuthorInvite(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<CancelCoAuthorInviteCommand, DraftVoki> handler
    ) {
        var request = httpContext.GetValidatedRequest<VokiCoAuthorActionRequest>();
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        CancelCoAuthorInviteCommand command = new(vokiId, request.ParsedUserId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<DraftVoki, VokiCoAuthorsWithInvitedResponse>(result);

    }

    private static async Task<IResult> AcceptCoAuthorInvite(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<AcceptCoAuthorInviteCommand> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        AcceptCoAuthorInviteCommand command = new(vokiId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }

    private static async Task<IResult> DeclineCoAuthorInvite(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<DeclineCoAuthorInviteCommand, ImmutableArray<VokiId>> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        DeclineCoAuthorInviteCommand command = new(vokiId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (vokiIds) => Results.Json(
            new { VokiIds = vokiIds.Select(id => id.ToString()).ToArray() })
        );
    }
}