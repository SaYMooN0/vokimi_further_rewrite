using GeneralVokiTakingService.Api.contracts;
using GeneralVokiTakingService.Api.contracts.voki_taking;
using GeneralVokiTakingService.Api.contracts.voki_taking.free_answering;
using GeneralVokiTakingService.Api.contracts.voki_taking.sequential_answering;
using GeneralVokiTakingService.Application.general_vokis.commands;
using GeneralVokiTakingService.Application.general_vokis.commands.free_answering_voki_taking;
using GeneralVokiTakingService.Application.general_vokis.commands.sequential_answering_voki_taking;
using GeneralVokiTakingService.Application.common.dtos;

namespace GeneralVokiTakingService.Api.endpoints;

internal class SpecificVokiHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/");

        group.MapPost("/start-taking", StartVokiTaking)
            .WithRequestValidation<StartVokiTakingRequest>();

        group.MapPost("/continue-taking", ContinueVokiTaking);

        group.MapPost("/free-answering/save-current-state", SaveCurrentFreeVokiTakingSessionState)
            .WithRequestValidation<SaveCurrentFreeVokiTakingSessionStateRequest>();
        
        group.MapPost("/free-answering/finish", FinishVokiTakingWithFreeAnswering)
            .WithRequestValidation<FinishVokiTakingWithFreeAnsweringRequest>();

        group.MapPost("/sequential-answering/finish", FinishVokiTakingWithSequentialAnswering)
            .WithRequestValidation<FinishVokiTakingWithSequentialAnsweringRequest>();

        group.MapPost("/sequential-answering/answer-question", AnswerQuestionForSequentialAnsweringSession)
            .WithRequestValidation<AnswerQuestionForSequentialAnsweringSessionRequest>();

        return group;
    }

    private static async Task<IResult> ContinueVokiTaking(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<ContinueVokiTakingCommand, VokiTakingData> handler
    )
    {
        VokiId id = httpContext.GetVokiIdFromRoute();

        ContinueVokiTakingCommand command = new(id);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, data => Results.Json(data));
    }

    private static async Task<IResult> StartVokiTaking(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<StartVokiTakingCommand, IStartVokiTakingCommandResult> handler
    )
    {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<StartVokiTakingRequest>();

        StartVokiTakingCommand command = new(id, request.TerminateCurrentActive);
        var result = await handler.Handle(command, ct);

        return result.Match(
            err => CustomResults.ErrorResponse(err),
            res =>
            {
                if (res is SuccessStartVokiTakingCommandResult success)
                {
                    return Results.Json(StartTakingResponse.Create(success.Data));
                }

                if (res is StartVokiTakingCommandActiveSessionResult active)
                {
                    return Results.Json(new
                    {
                        active.Id,
                        active.StartedAt
                    });
                }

                return Results.StatusCode(500);
            }
        );
    }

    private static async Task<IResult> FinishVokiTakingWithFreeAnswering(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<FinishVokiTakingWithFreeAnsweringCommand, GeneralVokiResultId> handler
    )
    {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<FinishVokiTakingWithFreeAnsweringRequest>();

        FinishVokiTakingWithFreeAnsweringCommand command = new(
            id, request.ParsedSessionId, request.ParsedChosenAnswers,
            request.ParsedSessionStartTime, request.ClientSessionFinishTime
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (resId) => Results.Json(
            new VokiTakingFinishedResponse(resId.ToString())
        ));
    }

    private static async Task<IResult> FinishVokiTakingWithSequentialAnswering(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<FinishVokiTakingWithSequentialAnsweringCommand, GeneralVokiResultId> handler
    )
    {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<FinishVokiTakingWithSequentialAnsweringRequest>();


        FinishVokiTakingWithSequentialAnsweringCommand command = new(
            vokiId, request.ParsedSessionId, request.ParsedSessionStartTime,
            ClientSessionFinishTime: request.ClientSessionFinishTime,
            request.ParsedLastQuestionId, request.LastQuestionOrderInVokiTaking,
            request.ParsedQuestionChosenAnswers, request.ParsedLastQuestionShownAt,
            LastQuestionClientAnsweredAt: request.ClientLastQuestionAnsweredAt
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (resId) => Results.Json(
            new VokiTakingFinishedResponse(resId.ToString())
        ));
    }

    private static async Task<IResult> AnswerQuestionForSequentialAnsweringSession(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<AnswerQuestionInSequentialAnsweringVokiTakingCommand,
            AnswerQuestionInSequentialAnsweringVokiTakingCommandResult> handler
    )
    {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<AnswerQuestionForSequentialAnsweringSessionRequest>();

        AnswerQuestionInSequentialAnsweringVokiTakingCommand command = new(
            vokiId, request.ParsedSessionId, request.ParsedQuestionId,
            ShownAt: request.ParsedShownAt,
            ClientQuestionAnsweredAt: request.ClientQuestionAnsweredAt,
            request.QuestionOrderInVokiTaking,
            request.ParsedChosenAnswers
        );
        var result = await handler.Handle(command, ct);

        return CustomResults
            .FromErrOrToJson<AnswerQuestionInSequentialAnsweringVokiTakingCommandResult,
                SequentialAnswerSessionNextQuestionResponse>(result);
    }
}