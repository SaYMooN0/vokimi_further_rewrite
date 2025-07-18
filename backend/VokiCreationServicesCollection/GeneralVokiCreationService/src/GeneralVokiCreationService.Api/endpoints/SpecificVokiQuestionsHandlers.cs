namespace GeneralVokiCreationService.Api.endpoints;

internal static class SpecificVokiQuestionsHandlers
{
    internal static void MapSpecificVokiQuestionsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/questions/{questionId}/");

        group.WithGroupAuthenticationRequired();

        // group.MapGet("/", GetVokiQuestionsOverview);
    }
}