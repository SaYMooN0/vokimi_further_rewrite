using VokisCatalogService.Api.catalog;
using VokisCatalogService.Application.vokis.queries;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.endpoints;

internal static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");
        group.MapGet("/overview", GetVokiOverviewInfo);

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
}