using ApiShared;
using ApiShared.extensions;
using ApplicationShared.messaging;
using VokiCommentsService.Api.contracts;
using VokiCommentsService.Application.app_users.queries;
using VokiCommentsService.Application.common.repositories;

namespace VokiCommentsService.Api.endpoints;

public static class RootHandlers
{
    internal static void MapRootHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/");
        
        group.MapGet("/commented-vokis", GetUserCommentedVokis)
            .WithAuthenticationRequired();
    }
    private static async Task<IResult> GetUserCommentedVokis(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ListUserCommentedVokiIdsQuery, VokiIdWithLastCommentedDateDto[]> handler
    ) {
        ListUserCommentedVokiIdsQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<VokiIdWithLastCommentedDateDto[], UserCommentedVokiIdsResponse>(result);
    }

} 