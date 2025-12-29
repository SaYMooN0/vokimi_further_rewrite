using VokiRatingsService.Api.contracts;

namespace VokiRatingsService.Api.endpoints;

internal class ManageVokiEndpoints : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/manage");

        group.MapGet("/ratings", GetManageVokiRatingsData);
       
    }

    private static async Task<IResult> GetManageVokiRatingsData(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<UserRatingsDataForVokiQuery, UserRatingsDataForVokiQueryResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();

        UserRatingsDataForVokiQuery query = new(vokiId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<UserRatingsDataForVokiQueryResult, UserRatingsDataForVokiResponse>(result);
    }
}