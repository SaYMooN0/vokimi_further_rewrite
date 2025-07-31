using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts.results;

public record class VokiResultDataResponse(
    string Id,
    string Name,
    string Text,
    string? Image
)
{
    public static VokiResultDataResponse Create(VokiResult result) => new(
        result.Id.ToString(),
        result.Name.ToString(),
        result.Text.ToString(),
        result.Image?.ToString() ?? null
    );
}