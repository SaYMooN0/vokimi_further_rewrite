using VokisCatalogService.Api.contracts.manage_voki;
using VokisCatalogService.Application.vokis.queries;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.endpoints.manage_voki;

public class ManageVokiCatalogPage : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/manage/catalog-page");

        group.MapGet("/", GetVokiCatalogPageSettings);
        // group.MapPatch("/update-recommendations", UpdateVokiCatalogPageRecommendations);
        
        return group;
    }

    private static async Task<IResult> GetVokiCatalogPageSettings(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<GetVokiQuery, BaseVoki> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        GetVokiQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<BaseVoki, VokiCatalogPageSettingsResponse>(result);
    }
}