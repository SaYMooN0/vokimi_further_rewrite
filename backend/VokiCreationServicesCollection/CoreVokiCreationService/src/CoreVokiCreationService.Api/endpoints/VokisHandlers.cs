using CoreVokiCreationService.Api.contracts.vokis_brief_info;
using CoreVokiCreationService.Application.draft_vokis.queries;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Api.endpoints;

public static class VokisHandlers
{
    internal static void MapVokisHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/");

        group.WithGroupAuthenticationRequired();

        group.MapPost("/brief-info", ListVokisBriefInfo) 
            .WithRequestValidation<ListVokisBriefInfoRequest>();
    }

    private static async Task<IResult> ListVokisBriefInfo(
        HttpContext httpContext, CancellationToken ct,
        IQueryHandler<ListVokisQuery, DraftVoki[]> handler
    ) {
        var request = httpContext.GetValidatedRequest<ListVokisBriefInfoRequest>();

        ListVokisQuery query = new(request.ParsedVokiIds);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (vokis) => Results.Json(new {
            Vokis = vokis.Select(VokiBriefInfoResponse.FromVoki).ToArray()
        }));
    }
}