using GeneralVokiTakingService.Application.general_vokis.queries;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Api.contracts.view_results;

public record ViewReceivedResultsResponse(
    ResultPreviewWithUserTakingsResponse[] Results,
    GeneralVokiResultsVisibility ResultsVisibility,
    string VokiName
)
{
    public static ViewReceivedResultsResponse Create(
        ImmutableArray<VokiResultWithTakingDates> results,
        GeneralVokiResultsVisibility resultsVisibility,
        VokiName vokiName
    ) => new(
        results.Select(ResultPreviewWithUserTakingsResponse.Create).ToArray(),
        resultsVisibility,
        vokiName.ToString()
    );
}

public record ResultPreviewWithUserTakingsResponse(
    string Id,
    string Name,
    string? Image,
    TakingPeriodResponse[] Takings
)
{
    public static ResultPreviewWithUserTakingsResponse Create(VokiResultWithTakingDates resWithTakings) => new(
        resWithTakings.Result.Id.ToString(),
        resWithTakings.Result.Name,
        resWithTakings.Result.Image?.ToString() ?? null,
        resWithTakings.VokiTakings
            .Select(t => new TakingPeriodResponse(t.Start, t.Finish))
            .ToArray()
    );
}

public record TakingPeriodResponse(DateTime Start, DateTime Finish);