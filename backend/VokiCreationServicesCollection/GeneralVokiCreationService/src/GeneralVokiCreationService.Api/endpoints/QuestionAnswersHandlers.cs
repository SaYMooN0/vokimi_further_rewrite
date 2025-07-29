using GeneralVokiCreationService.Api.contracts.answers;
using GeneralVokiCreationService.Api.contracts.questions;
using GeneralVokiCreationService.Api.contracts.questions.update_question;
using GeneralVokiCreationService.Api.extensions;
using GeneralVokiCreationService.Application.draft_vokis.commands.questions;
using GeneralVokiCreationService.Application.draft_vokis.commands.questions.answers;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Infrastructure.parsers;

namespace GeneralVokiCreationService.Api.endpoints;

internal static class QuestionAnswersHandlers
{
    internal static void MapQuestionAnswersHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/questions/{questionId}/answers/");

        group.WithGroupAuthenticationRequired();

        group.MapPost("/add-new", AddNewAnswerToVokiQuestion)
            .WithRequestValidation<AddNewAnswerToVokiQuestionRequest>();
    }

    private static async Task<IResult> AddNewAnswerToVokiQuestion(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<AddNewAnswerToVokiQuestionCommand, VokiQuestionAnswer> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
        var request = httpContext.GetValidatedRequest<AddNewAnswerToVokiQuestionRequest>();

        AddNewAnswerToVokiQuestionCommand command = new(id, questionId, request.ParsedAnswerData);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (answer) => Results.Json(
            VokiQuestionAnswerResponse.Create(answer)
        ));
    }
}