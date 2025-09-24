using GeneralVokiTakingService.Application.general_vokis.queries;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Api.contracts.view_results;

public record class ViewSingleResultResponse(
    string Id,
    string Name,
    string Text,
    string? Image,
    GeneralVokiResultsVisibility ResultsVisibility,
    string VokiName,
    uint ResultsCount
) : ICreatableResponse<ViewVokiResultQueryResult>
{
    public static ICreatableResponse<ViewVokiResultQueryResult> Create(ViewVokiResultQueryResult queryResult) =>
        new ViewSingleResultResponse(
            queryResult.Result.Id.ToString(),
            queryResult.Result.Name,
            queryResult.Result.Text,
            queryResult.Result.Image?.ToString() ?? null,
            queryResult.ResultsVisibility,
            queryResult.VokiName.ToString(),
            queryResult.TotalResultsCount
        );
}