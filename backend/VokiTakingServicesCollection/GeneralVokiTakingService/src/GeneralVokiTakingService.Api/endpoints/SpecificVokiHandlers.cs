namespace GeneralVokiTakingService.Api.endpoints;

internal static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        group.WithGroupAuthenticationRequired();

        // group.MapGet("/", GetVokiTakingData);
    }

    // private static async Task<IResult> GetVokiTakingData(
    //     CancellationToken ct, HttpContext httpContext,
    //     IQueryHandler<GetVokiQuery, DraftGeneralVoki> handler
    // ) {
    //     VokiId id = httpContext.GetVokiIdFromRoute();
    //
    //     GetVokiQuery query = new(id);
    //     var result = await handler.Handle(query, ct);
    //
    //     return CustomResults.FromErrOr(result, (voki) => Results.Json(
    //         VokiTakingDataResponse.Create(voki)
    //     ));
    // }
}