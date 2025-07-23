using GeneralVokiCreationService.Api.contracts.answers;

namespace GeneralVokiCreationService.Api.endpoints;

internal static class QuestionAnswersHandlers
{
    internal static void MapQuestionAnswersHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/questions/{questionId}/answers/");

        group.WithGroupAuthenticationRequired();

        // group.MapPost("/add-new", AddNewAnswerToVokiQuestion)
        //     .WithRequestValidation<AddNewAnswerToVokiQuestionRequest>();
    }
    // private static async Task<IResult> AddNewAnswerToVokiQuestion(
    //     CancellationToken ct, HttpContext httpContext,
    //     ICommandHandler<AddNewAnswerToVokiQuestionCommand, GeneralVokiQuestionId> handler
    // ) {
    //     
    // }
}