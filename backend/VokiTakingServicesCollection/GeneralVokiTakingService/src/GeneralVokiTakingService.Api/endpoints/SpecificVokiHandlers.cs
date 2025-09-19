using GeneralVokiTakingService.Api.contracts;
using GeneralVokiTakingService.Api.contracts.finish_voki_taking;
using GeneralVokiTakingService.Api.extensions;
using GeneralVokiTakingService.Application.general_vokis.commands;
using GeneralVokiTakingService.Application.general_vokis.commands.finish_voki_taking;
using GeneralVokiTakingService.Application.general_vokis.queries;
using GeneralVokiTakingService.Domain.general_voki_aggregate;

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

        // group.MapPost("/sequential-answering/answer-question", AnswerQuestionForSequentialAnsweringSession)
        //     .WithRequestValidation<FinishVokiTakingWithSequentialAnsweringRequest>();

        group.MapGet("/results/{resultId}", ViewVokiResult);
    }

    private static async Task<IResult> StartVokiTaking(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<StartVokiTakingCommand, StartVokiTakingCommandResponse> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        StartVokiTakingCommand command = new(id);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (vokiTakingData) => Results.Json(
            GeneralVokiTakingResponse.Create(vokiTakingData)
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

    // private static async Task<IResult> AnswerQuestionForSequentialAnsweringSession(
    //     CancellationToken ct, HttpContext httpContext
    // ) {
    //     VokiId id = httpContext.GetVokiIdFromRoute();
    //     var request = httpContext.GetValidatedRequest<FinishVokiTakingWithSequentialAnsweringRequest>();
    //
    // }
    private static async Task<IResult> ViewVokiResult(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<ViewVokiResultQuery, VokiResult> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiResultId resultId = httpContext.GetResultIdFromRoute();


        ViewVokiResultQuery command = new(vokiId, resultId);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (vokiResult) => Results.Json(
            VokiResultViewResponse.Create(vokiResult)
        ));
    }
}