using GeneralVokiCreationService.Api.contracts.answers;
using GeneralVokiCreationService.Api.contracts.questions;
using GeneralVokiCreationService.Api.extensions;
using GeneralVokiCreationService.Application.draft_vokis.commands.answers;
using GeneralVokiCreationService.Application.draft_vokis.commands.questions;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.endpoints;

internal static class QuestionAnswersHandlers
{
    internal static void MapQuestionAnswersHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/questions/{questionId}/answers/");

        group.WithGroupAuthenticationRequired();

        group.MapPost("/add-new", AddNewAnswerToVokiQuestion)
            .WithRequestValidation<SaveVokiQuestionAnswerRequest>();

        group.MapPut("/{answerId}/update", UpdateVokiQuestionAnswer)
            .WithRequestValidation<SaveVokiQuestionAnswerRequest>();

        group.MapDelete("/{answerId}/delete", DeleteVokiQuestionAnswer);
    }

    private static async Task<IResult> AddNewAnswerToVokiQuestion(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<AddNewAnswerToVokiQuestionCommand, VokiQuestionAnswer> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
        var request = httpContext.GetValidatedRequest<SaveVokiQuestionAnswerRequest>();

        AddNewAnswerToVokiQuestionCommand command = new(
            id, questionId, request.ParsedAnswerData, request.ParsedResultIds);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (answer) => Results.Json(
            VokiQuestionAnswerResponse.Create(answer)
        ));
    }

    private static async Task<IResult> UpdateVokiQuestionAnswer(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiQuestionAnswerCommand, VokiQuestionAnswer> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
        GeneralVokiAnswerId answerId = httpContext.GetAnswerIdFromRoute();
        var request = httpContext.GetValidatedRequest<SaveVokiQuestionAnswerRequest>();

        UpdateVokiQuestionAnswerCommand command = new(
            vokiId, questionId, answerId, request.ParsedAnswerData, request.ParsedResultIds
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (answer) => Results.Json(
            VokiQuestionAnswerResponse.Create(answer)
        ));
    }

    private static async Task<IResult> DeleteVokiQuestionAnswer(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<DeleteVokiQuestionAnswerCommand> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
        GeneralVokiAnswerId answerId = httpContext.GetAnswerIdFromRoute();

        DeleteVokiQuestionAnswerCommand command = new(vokiId, questionId, answerId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, CustomResults.Deleted);
    }
}