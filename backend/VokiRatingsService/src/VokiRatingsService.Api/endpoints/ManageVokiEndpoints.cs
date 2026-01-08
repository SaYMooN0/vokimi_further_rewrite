using ApiShared.extensions;
using VokiRatingsService.Api.contracts.manage_voki;
using VokiRatingsService.Application.vokis.queries;
using VokiRatingsService.Domain.common;

namespace VokiRatingsService.Api.endpoints;

internal class ManageVokiEndpoints : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/manage");

        group.MapGet("/overview", ManageVokiRatingsOverview);
        group.MapGet("/history", ManageVokiRatingsHistory);
    }

    private static async Task<IResult> ManageVokiRatingsOverview(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ManageVokiRatingsOverviewQuery, VokiRatingsDistribution> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        ManageVokiRatingsOverviewQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<VokiRatingsDistribution, ManageVokiRatingsOverviewResponse>(result);
    }
    private static async Task<IResult> ManageVokiRatingsHistory(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ManageVokiRatingsHistoryQuery, ManageVokiRatingsHistoryQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        ManageVokiRatingsHistoryQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<ManageVokiRatingsHistoryQueryResult, ManageVokiRatingsHistoryResponse>(result);
    }
}