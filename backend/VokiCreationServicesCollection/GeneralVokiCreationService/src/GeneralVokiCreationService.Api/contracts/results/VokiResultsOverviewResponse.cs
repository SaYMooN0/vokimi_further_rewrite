using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts.results;

public record class VokiResultsOverviewResponse(
    VokiResultDataResponse[] Results
)
{
    public static VokiResultsOverviewResponse Create(ImmutableArray<VokiResult> results) => new(
        results
            .OrderBy(r => r.CreationDate)
            .Select(VokiResultDataResponse.Create)
            .ToArray()
    );
}