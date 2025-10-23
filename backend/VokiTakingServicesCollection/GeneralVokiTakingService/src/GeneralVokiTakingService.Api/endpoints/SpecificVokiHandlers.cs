using GeneralVokiTakingService.Api.contracts;
using GeneralVokiTakingService.Api.contracts.finish_voki_taking;
using GeneralVokiTakingService.Application.general_vokis.commands;
using GeneralVokiTakingService.Application.general_vokis.commands.free_answering_voki_taking;
using GeneralVokiTakingService.Application.general_vokis.commands.sequential_answering_voki_taking;

namespace GeneralVokiTakingService.Api.endpoints;

internal static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        group.MapPost("/start-taking", StartVokiTaking);

        group.MapPost("/free-answering/finish", FinishVokiTakingWithFreeAnswering)
            .WithRequestValidation<FinishVokiTakingWithFreeAnsweringRequest>();

        group.MapPost("/sequential-answering/finish", FinishVokiTakingWithSequentialAnswering)
            .WithRequestValidation<FinishVokiTakingWithSequentialAnsweringRequest>();

        group.MapPost("/sequential-answering/answer-question", AnswerQuestionForSequentialAnsweringSession)
            .WithRequestValidation<AnswerQuestionForSequentialAnsweringSessionRequest>();
    }

    private static async Task<IResult> StartVokiTaking(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<StartVokiTakingCommand, StartVokiTakingCommandResponse> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        StartVokiTakingCommand command = new(id);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (vokiTakingData) => Results.Json(
            StartTakingResponse.Create(vokiTakingData)
        ));
    }

    private static async Task<IResult> FinishVokiTakingWithFreeAnswering(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<FinishVokiTakingWithFreeAnsweringCommand, GeneralVokiResultId> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<FinishVokiTakingWithFreeAnsweringRequest>();

        FinishVokiTakingWithFreeAnsweringCommand command = new(
            id, request.ParsedSessionId, request.ParsedChosenAnswers,
            request.ClientStartTime,
            request.ServerStartTime,
            request.ClientFinishTime
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (resId) => Results.Json(
            new VokiTakingFinishedResponse(resId.ToString())
        ));
    }

    private static async Task<IResult> FinishVokiTakingWithSequentialAnswering(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<FinishVokiTakingWithSequentialAnsweringCommand, GeneralVokiResultId> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        FinishVokiTakingWithSequentialAnsweringCommand command = new(id);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (resId) => Results.Json(
            new VokiTakingFinishedResponse(resId.ToString())
        ));
    }

    private static async Task<IResult> AnswerQuestionForSequentialAnsweringSession(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<AnswerQuestionInSequentialAnsweringVokiTakingCommand, VokiTakingQuestionData> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<AnswerQuestionForSequentialAnsweringSessionRequest>();
        
        AnswerQuestionInSequentialAnsweringVokiTakingCommand command = new(vokiId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrToJson<VokiTakingQuestionData, GeneralVokiTakingResponseQuestionData>(result);
    }
}