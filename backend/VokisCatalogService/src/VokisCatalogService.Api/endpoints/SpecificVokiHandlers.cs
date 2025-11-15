using SharedKernel.common.vokis;
using VokisCatalogService.Api.contracts;
using VokisCatalogService.Application.vokis.queries;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.endpoints;

internal class SpecificVokiHandlers : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/");

        group.MapGet("/overview", GetVokiOverviewInfo);
        group.MapGet("/does-exist", CheckIfVokiExists);
    }

    private static async Task<IResult> GetVokiOverviewInfo(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiQuery, BaseVoki> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        GetVokiQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (voki) => Results.Json(
            VokiOverviewResponse.Create(voki)
        ));
    }

    private static async Task<IResult> CheckIfVokiExists(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<GetVokiTypeQuery, VokiType> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        GetVokiTypeQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, vokiType => Results.Json(new { VokiType = vokiType }));
    }
}