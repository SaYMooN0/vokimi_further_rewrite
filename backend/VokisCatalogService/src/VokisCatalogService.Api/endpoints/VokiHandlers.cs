using VokisCatalogService.Api.contracts;
using VokisCatalogService.Application.vokis.queries;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.endpoints;

internal static class VokiHandlers
{
    internal static void MapVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/");

        group.MapGet("/all", ListAllVokis);
        group.MapGet("/list-user-voki-ids", ListUserVokiIds);
        group.MapPost("/brief-info", ListVokisBriefInfo)
            .WithRequestValidation<ListVokisBriefInfoRequest>();
    }

    private static async Task<IResult> ListUserVokiIds(
        CancellationToken ct, IQueryHandler<ListUserVokiIdsQuery, VokiId[]> handler
    ) {
        ListUserVokiIdsQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (vokiIds) => Results.Json(
            new { VokiIds = vokiIds.Select(v => v.ToString()).ToArray() }
        ));
    }

    private static async Task<IResult> ListAllVokis(
        CancellationToken ct, IQueryHandler<ListAllVokisQuery, BaseVoki[]> handler
    ) {
        ListAllVokisQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (vokis) => Results.Json(
            MultipleVokisBriefInfoResponse.Create(vokis)
        ));
    }

    private static async Task<IResult> ListVokisBriefInfo(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<ListVokisWithIdsQuery, BaseVoki[]> handler
    ) {
        var request = httpContext.GetValidatedRequest<ListVokisBriefInfoRequest>();

        ListVokisWithIdsQuery withIdsQuery = new(request.ParsedVokiIds);
        var result = await handler.Handle(withIdsQuery, ct);

        return CustomResults.FromErrOr(result, (vokis) => Results.Json(
            MultipleVokisBriefInfoResponse.Create(vokis)
        ));
    }
}