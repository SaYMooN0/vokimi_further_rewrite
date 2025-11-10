using VokisCatalogService.Api.contracts;
using VokisCatalogService.Application.vokis.queries;

namespace VokisCatalogService.Api.endpoints;

internal class RootHandlers : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/");

        group.MapGet("/taken-vokis", GetUserTakenVokis)
            .WithAuthenticationRequired();
    }

    private static async Task<IResult> GetUserTakenVokis(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ListTakenVokiIdsQuery, VokiIdWithLastTakenDateDto[]> handler
    ) {
        ListTakenVokiIdsQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<VokiIdWithLastTakenDateDto[], UserTakenVokiIdsResponse>(result);
    }
}