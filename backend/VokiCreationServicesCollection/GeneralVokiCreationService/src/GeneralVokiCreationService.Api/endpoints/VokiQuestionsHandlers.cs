using System.Collections.Immutable;
using GeneralVokiCreationService.Api.contracts.questions;
using GeneralVokiCreationService.Application.draft_vokis.commands.questions;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Api.endpoints;

internal static class VokiQuestionsHandlers
{
    internal static void MapVokiQuestionsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/questions");

        group.WithGroupAuthenticationRequired();

        group.MapGet("/overview", GetVokiQuestionsOverview);
        group.MapPost("/add-new", AddNewQuestionToVoki);
    }

    private static async Task<IResult> GetVokiQuestionsOverview(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiWithQuestionsQuery, DraftGeneralVoki> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        GetVokiWithQuestionsQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (voki) => Results.Json(
            VokiQuestionsOverviewResponse.Create(voki)
        ));
    }

    private static async Task<IResult> AddNewQuestionToVoki(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<AddNewQuestionToVokiCommand, GeneralVokiQuestionId> handler,
        [FromBody] GeneralVokiAnswerType questionAnswersType
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        AddNewQuestionToVokiCommand command = new(id, questionAnswersType);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (questionId) => Results.Json(
            new { Id = questionId.ToString() }
        ));
    }
}