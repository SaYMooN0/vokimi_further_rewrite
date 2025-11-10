using ApiShared.extensions;
using VokiRatingsService.Api.contracts;
using VokiRatingsService.Application.app_users.queries;
using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Api.endpoints;

internal class RootHandlers : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/");

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