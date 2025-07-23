namespace GeneralVokiCreationService.Api.endpoints;

internal static class SpecificQuestionAnswerHandlers
{
    internal static void MapSpecificQuestionAnswerHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/questions/{questionId}/answers/{answerId}/");

        group.WithGroupAuthenticationRequired();

        // group.MapGet("/overview", GetVokiQuestionsOverview);
    }
}