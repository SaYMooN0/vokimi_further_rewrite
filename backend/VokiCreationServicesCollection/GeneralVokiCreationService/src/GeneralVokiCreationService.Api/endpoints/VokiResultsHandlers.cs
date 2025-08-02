using GeneralVokiCreationService.Api.contracts.results;
using GeneralVokiCreationService.Application.draft_vokis.commands.results;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.endpoints;

internal static class VokiResultsHandlers
{
    internal static void MapVokiResultsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/results/");

        group.WithGroupAuthenticationRequired();

        group.MapGet("/overview", GetVokiResultsOverview);
        group.MapPost("/add-new", AddNewResultToVoki)
            .WithRequestValidation<AddNewResultToVokiRequest>();
        group.MapGet("/ids-names", GetVokiResultsIdsWithNames);
    }

    private static async Task<IResult> GetVokiResultsOverview(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiWithResultsQuery, DraftGeneralVoki> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        GetVokiWithResultsQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (voki) => Results.Json(
            VokiResultsOverviewResponse.Create(voki.Results)
        ));
    }

    private static async Task<IResult> AddNewResultToVoki(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<AddNewResultToVokiCommand, ImmutableArray<VokiResult>> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<AddNewResultToVokiRequest>();

        AddNewResultToVokiCommand command = new(id, request.ParsedName);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (results) => Results.Json(
            VokiResultsOverviewResponse.Create(results)
        ));
    }

    private static async Task<IResult> GetVokiResultsIdsWithNames(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiWithResultsQuery, DraftGeneralVoki> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        GetVokiWithResultsQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (voki) => Results.Json(new {
                Results = voki.Results.Select(VokiResultIdWithNameResponse.Create).ToArray()
            }
        ));
    }
}