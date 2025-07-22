using GeneralVokiCreationService.Api.contracts.questions;
using GeneralVokiCreationService.Api.extensions;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.endpoints;

internal static class SpecificVokiQuestionsHandlers
{
    internal static void MapSpecificVokiQuestionsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/questions/{questionId}/");

        group.WithGroupAuthenticationRequired();

        group.MapGet("/", GetVokiQuestionFullData);
        // group.MapPatch("/update", UpdateVokiQuestion);
        // group.MapDelete("/delete", DeleteVokiQuestion);
        // group.MapPatch("/move-up-in-order", MoveQuestionUpInOrder);
        // group.MapPatch("/move-down-in-order", MoveQuestionDownInOrder);
    }

    private static async Task<IResult> GetVokiQuestionFullData(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiQuestionWithAnswersQuery, VokiQuestion> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();

        GetVokiQuestionWithAnswersQuery query = new(vokiId, questionId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (question) => Results.Json(
            VokiQuestionFullDataResponse.Create(question)
        ));
    }
    // private static async Task<IResult> MoveQuestionUpInOrder(
    //     CancellationToken ct, HttpContext httpContext,
    //     ICommandHandler<MoveQuestionUpInOrderCommand, ImmutableDictionary<GeneralVokiQuestionId, ushort>> handler
    // ) {
    //     VokiId id = httpContext.GetVokiIdFromRoute();
    //     GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
    //
    //
    //     MoveQuestionUpInOrderCommand command = new(id, questionId);
    //     var result = await handler.Handle(command, ct);
    //
    //     return CustomResults.FromErrOr(result, (questionsOrder) => Results.Json(
    //         new { Id = questionId.ToString() }
    //     ));
    // }
}