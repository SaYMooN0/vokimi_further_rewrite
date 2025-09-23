using GeneralVokiTakingService.Domain.general_voki_aggregate.dtos;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Api.contracts.view_results;

public record class ViewAllVokiResultsResponse(
    VokiResultPreviewWithPercentageResponse[] Results,
    bool ShowResultsDistribution,
    GeneralVokiResultsVisibility ResultsVisibility,
    string VokiName
)
{
    public static ViewAllVokiResultsResponse Create(
        IEnumerable<VokiResultWithDistributionPercent> results,
        bool allowResultsPercentage,
        GeneralVokiResultsVisibility resultsVisibility,
        VokiName vokiName
    ) => new(
        results.Select(VokiResultPreviewWithPercentageResponse.Create).ToArray(),
        allowResultsPercentage,
        resultsVisibility,
        vokiName.ToString()
    );
}

public record class VokiResultPreviewWithPercentageResponse(
    string Id,
    string Name,
    string? Image,
    double DistributionPercent
)
{
    public static VokiResultPreviewWithPercentageResponse Create(VokiResultWithDistributionPercent resWithPercentage) => new(
        resWithPercentage.Result.Id.ToString(),
        resWithPercentage.Result.Name,
        resWithPercentage.Result.Image?.ToString() ?? null,
        resWithPercentage.DistributionPercent
    );
}