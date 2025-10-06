using ApiShared.extensions;
using VokiRatingsService.Api.contracts;
using VokiRatingsService.Application.app_users.queries;
using VokiRatingsService.Application.common.repositories;

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
        IQueryHandler<ListUserRatedVokiIdsQuery, VokiIdWithRatingDateDto[]> handler
    ) {
        ListUserRatedVokiIdsQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<VokiIdWithRatingDateDto[], UserRatedVokiIdsResponse>(result);
    }
}