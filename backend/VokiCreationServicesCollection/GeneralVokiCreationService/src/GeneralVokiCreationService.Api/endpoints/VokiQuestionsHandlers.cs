using GeneralVokiCreationService.Api.contracts.questions;
using GeneralVokiCreationService.Application.draft_vokis.commands.questions;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.endpoints;

internal class VokiQuestionsHandlers : IEndpointGroup
{
    public RouteGroupBuilder MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = routeBuilder.MapGroup("/vokis/{vokiId}/questions/");

        group.MapGet("/overview", GetVokiQuestionsOverview);
        group.MapPost("/add-new", AddNewQuestionToVoki)
            .WithRequestValidation<AddNewQuestionToVokiRequest>();
        
        return group;
    }

    private static async Task<IResult> GetVokiQuestionsOverview(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiWithQuestionsQuery, DraftGeneralVoki> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        GetVokiQuestionsOverviewQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (voki) => Results.Json(
            VokiQuestionsOverviewResponse.Create(voki)
        ));
    }

    private static async Task<IResult> AddNewQuestionToVoki(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<AddNewQuestionToVokiCommand, GeneralVokiQuestionId> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<AddNewQuestionToVokiRequest>();

        AddNewQuestionToVokiCommand command = new(id, request.QuestionAnswersType);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (questionId) => Results.Json(
            new { Id = questionId.ToString() }
        ));
    }
}