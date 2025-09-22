using GeneralVokiTakingService.Api.contracts.view_results;
using GeneralVokiTakingService.Api.extensions;
using GeneralVokiTakingService.Application.general_vokis.queries;
using GeneralVokiTakingService.Domain.general_voki_aggregate;

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
        IQueryHandler<ViewVokiResultQuery, VokiResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();


        ViewVokiResultQuery query = new(vokiId, resultId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (vokiResult) => Results.Json(
            VokiResultViewResponse.Create(vokiResult)
        ));
    }

    private static async Task<IResult> ViewAllVokiResults(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ViewAllVokiResultsQuery, ViewAllVokiResultsQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        ViewAllVokiResultsQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (r) => Results.Json(
            ViewAllVokiResultsResponse.Create(r.Results, r.ShowResultsDistribution, r.ResultsVisibility)
        ));
    }

    private static async Task<IResult> ViewVokiReceivedResults(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<VokiReceivedResultsQuery, VokiReceivedResultsQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        VokiReceivedResultsQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (r) => Results.Json(
            ViewReceivedResultsResponse.Create(r)
        ));
    }
}