using System.Collections.Immutable;
using GeneralVokiCreationService.Api.contracts.questions;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.endpoints;

internal static class VokiQuestionsHandlers
{
    internal static void MapVokiQuestionsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/questions");

        group.WithGroupAuthenticationRequired();

        group.MapGet("/overview", GetVokiQuestionsOverview);
    }

    private static async Task<IResult> GetVokiQuestionsOverview(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ListVokiQuestionsQuery, ImmutableArray<VokiQuestion>> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        ListVokiQuestionsQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (questions) => Results.Json(
            new { Questions = questions.Select(VokiQuestionOverviewResponse.Create).ToArray() }
        ));
    }
}