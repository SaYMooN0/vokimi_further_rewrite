using ApiShared.extensions;
using VokiRatingsService.Api.contracts;
using VokiRatingsService.Application.app_users.queries;

namespace VokiRatingsService.Api.endpoints;

public static class RootHandlers
{
    internal static void MapRootHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/");
        
        group.MapGet("/rated-vokis", GetUserRatedVokis)
            .WithAuthenticationRequired();
    }
    private static async Task<IResult> GetUserRatedVokis(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ListUserVokiIdsQuery, ImmutableHashSet<VokiRatingId>> handler
    ) {
        ListUserVokiIdsQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<ImmutableHashSet<VokiRatingId>, UserRatingIdsResponse>(result);
    }

}