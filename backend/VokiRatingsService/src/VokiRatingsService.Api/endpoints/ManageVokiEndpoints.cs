using ApiShared.extensions;
using VokiRatingsService.Api.contracts.manage_voki;
using VokiRatingsService.Application.vokis.queries;
using VokiRatingsService.Domain.common;

namespace VokiRatingsService.Api.endpoints;

internal class ManageVokiEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/manage");

        group.MapGet("/ratings", GetManageVokiRatingsData);
       
    }

    private static async Task<IResult> GetManageVokiRatingsData(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ManageVokiRatingsDistributionQuery, VokiRatingsDistribution> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        ManageVokiRatingsDistributionQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<VokiRatingsDistribution, ManageVokiResponse>(result);
    }
}