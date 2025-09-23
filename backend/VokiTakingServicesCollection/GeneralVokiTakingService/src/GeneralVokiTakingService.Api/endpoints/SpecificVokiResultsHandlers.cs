using GeneralVokiTakingService.Api.contracts.view_results;
using GeneralVokiTakingService.Api.extensions;
using GeneralVokiTakingService.Application.general_vokis.queries;

namespace GeneralVokiTakingService.Api.endpoints;

internal static class SpecificVokiResultsHandlers
{
    internal static void MapSpecificVokiResultsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/results");

        group.MapGet("/{resultId}", ViewSpecificResult); //return results visibility

        group.MapGet("/all", ViewAllVokiResults);

        group.MapGet("/received", ViewVokiReceivedResults)
            .WithAuthenticationRequired();
    }

    private static async Task<IResult> ViewSpecificResult(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ViewVokiResultQuery, ViewVokiResultQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();


        ViewVokiResultQuery query = new(vokiId, resultId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (success) => Results.Json(
            ViewSingleResultResponse.Create(success.Result, success.ResultsVisibility, success.VokiName)
        ));
    }

    private static async Task<IResult> ViewAllVokiResults(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ViewAllVokiResultsQuery, ViewAllVokiResultsQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        ViewAllVokiResultsQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (success) => Results.Json(
            ViewAllVokiResultsResponse.Create(
                success.Results,
                success.ShowResultsDistribution,
                success.ResultsVisibility,
                success.VokiName
            )
        ));
    }

    private static async Task<IResult> ViewVokiReceivedResults(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<VokiReceivedResultsQuery, VokiReceivedResultsQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        VokiReceivedResultsQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (success) => Results.Json(
            ViewReceivedResultsResponse.Create(success.Results, success.ResultsVisibility, success.VokiName)
        ));
    }
}