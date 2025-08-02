using GeneralVokiCreationService.Api.contracts.answers;
using GeneralVokiCreationService.Api.contracts.questions;
using GeneralVokiCreationService.Api.extensions;
using GeneralVokiCreationService.Application.draft_vokis.commands.answers;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.endpoints;

internal static class SpecificQuestionAnswerHandlers
{
    internal static void MapSpecificQuestionAnswerHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/questions/{questionId}/answers/{answerId}/");

        group.WithGroupAuthenticationRequired();

        group.MapPut("/update", UpdateVokiQuestionAnswer)
            .WithRequestValidation<SaveVokiQuestionAnswerRequest>();
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
}