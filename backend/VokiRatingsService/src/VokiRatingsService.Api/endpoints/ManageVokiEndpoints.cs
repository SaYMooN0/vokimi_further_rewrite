using ApiShared.extensions;
using VokiRatingsService.Api.contracts.manage_voki;
using VokiRatingsService.Application.vokis.queries;

namespace VokiRatingsService.Api.endpoints;

internal class ManageVokiEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/manage");

        group.MapGet("/overview", ManageVokiRatingsOverview);
        group.MapGet("/snapshots", ManageVokiRatingsSnapshots);
    }

    private static async Task<IResult> ManageVokiRatingsOverview(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ManageVokiRatingsOverviewQuery, ManageVokiRatingsOverviewQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        ManageVokiRatingsOverviewQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<ManageVokiRatingsOverviewQueryResult, ManageVokiRatingsOverviewResponse>(result);
    }
    private static async Task<IResult> ManageVokiRatingsSnapshots(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ManageVokiRatingsSnapshotsQuery, ManageVokiRatingsSnapshotsQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        ManageVokiRatingsSnapshotsQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<ManageVokiRatingsSnapshotsQueryResult, >(result);
    }
}