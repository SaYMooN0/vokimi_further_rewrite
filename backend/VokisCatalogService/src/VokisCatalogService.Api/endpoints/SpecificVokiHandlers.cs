using VokisCatalogService.Api.contracts;
using VokisCatalogService.Application.vokis.queries;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.endpoints;

internal class SpecificVokiHandlers : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/");

        group.MapGet("/overview", GetVokiOverviewInfo);
    }

    private static async Task<IResult> GetVokiOverviewInfo(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiQuery, BaseVoki> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        GetVokiQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<BaseVoki, VokiOverviewResponse>(result);
    }
}