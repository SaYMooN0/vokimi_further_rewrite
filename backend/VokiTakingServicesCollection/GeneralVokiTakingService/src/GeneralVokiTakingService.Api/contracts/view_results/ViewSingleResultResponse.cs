using GeneralVokiTakingService.Domain.general_voki_aggregate;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Api.contracts.view_results;

public record class ViewSingleResultResponse(
    string Id,
    string Name,
    string Text,
    string? Image,
    GeneralVokiResultsVisibility ResultsVisibility,
    string VokiName
)
{
    public static ViewSingleResultResponse Create(
        VokiResult result,
        GeneralVokiResultsVisibility resultsVisibility,
        VokiName vokiName
    ) => new(
        result.Id.ToString(),
        result.Name,
        result.Text,
        result.Image?.ToString() ?? null,
        resultsVisibility,
        vokiName.ToString()
    );
}