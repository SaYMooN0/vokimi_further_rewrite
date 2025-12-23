using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.contracts.manage_voki;

public record VokiCatalogPageSettingsResponse() : ICreatableResponse<BaseVoki>
{
    public static ICreatableResponse<BaseVoki> Create(BaseVoki success) => new VokiCatalogPageSettingsResponse(
    );
}