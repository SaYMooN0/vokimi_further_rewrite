using GeneralVokiTakingService.Application.general_vokis.queries;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Api.contracts.view_results;

public record ViewReceivedResultsResponse(
    ResultPreviewWithUserTakingsResponse[] Results,
    GeneralVokiResultsVisibility ResultsVisibility,
    string VokiName,
    uint ResultsCount
) : ICreatableResponse<VokiReceivedResultsQueryResult>
{
    public static ICreatableResponse<VokiReceivedResultsQueryResult> Create(
        VokiReceivedResultsQueryResult queryResult
    ) => new ViewReceivedResultsResponse(
        queryResult.Results.Select(ResultPreviewWithUserTakingsResponse.Create).ToArray(),
        queryResult.ResultsVisibility,
        queryResult.VokiName.ToString(),
        queryResult.TotalResultsCount
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