using CoreVokiCreationService.Api.contracts;
using CoreVokiCreationService.Application.app_users.queries;
using CoreVokiCreationService.Application.draft_vokis.commands;
using CoreVokiCreationService.Application.draft_vokis.queries;
using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Api.endpoints;

public static class RootHandlers
{
    internal static void MapRootHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/");

        group.WithGroupAuthenticationRequired();

        group.MapGet("/list-user-voki-ids", ListUserVokiIds);
        group.MapPost("/initialize-new-voki", InitializeNewVoki)
            .WithRequestValidation<InitializeNewVokiRequest>();
        group.MapGet("/list-invites", ListUserInvites);
    }

    private static async Task<IResult> ListUserVokiIds(
        CancellationToken ct, IQueryHandler<ListUserVokiIdsQuery, ImmutableArray<VokiId>> handler
    ) {
        ListUserVokiIdsQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (vokiIds) => Results.Json(
            new { VokiIds = vokiIds.Select(v => v.ToString()).ToArray() }
        ));
    }

    private static async Task<IResult> InitializeNewVoki(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<InitializeNewVokiCommand, DraftVoki> handler
    ) {
        var request = httpContext.GetValidatedRequest<InitializeNewVokiRequest>();

        InitializeNewVokiCommand command = new(request.VokiType, request.ParseVokiName);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result,
            (voki) => CustomResults.Created(new {
                Id = voki.Id.ToString(),
                Type = voki.Type,
                Name = voki.Name.ToString()
            })
        );
    }

    private static async Task<IResult> ListUserInvites(
        CancellationToken ct, IQueryHandler<GetCurrentUserQuery, AppUser> handler
    ) {
        GetCurrentUserQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (user) => Results.Json(
            new { InvitedToCoAuthorVokiIds = user.InvitedToCoAuthorVokiIds.Select(v => v.ToString()).ToArray() }
        ));
    }
}