using GeneralVokiTakingService.Domain.general_voki_aggregate;

namespace GeneralVokiTakingService.Api.contracts;

public record class VokiResultViewResponse(
    string Id,
    string Name,
    string Text,
    string? Image
)
{
    public static VokiResultViewResponse Create(VokiResult result) => new(
        result.Id.ToString(),
        result.Name,
        result.Text,
        result.Image?.ToString() ?? null
    );
}
