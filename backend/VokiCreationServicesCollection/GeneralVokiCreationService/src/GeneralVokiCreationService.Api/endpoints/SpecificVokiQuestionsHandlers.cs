using GeneralVokiCreationService.Api.contracts.questions;
using GeneralVokiCreationService.Api.contracts.questions.update_question;
using GeneralVokiCreationService.Api.extensions;
using GeneralVokiCreationService.Application.draft_vokis.commands.questions;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using Microsoft.AspNetCore.Mvc;
using VokimiStorageKeysLib.draft_general_voki.question_image;

namespace GeneralVokiCreationService.Api.endpoints;

internal static class SpecificVokiQuestionsHandlers
{
    internal static void MapSpecificVokiQuestionsHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/questions/{questionId}/");

        group.WithGroupAuthenticationRequired();

        group.MapGet("/", GetVokiQuestionFullData);
        group.MapPatch("/update-text", UpdateQuestionText)
            .WithRequestValidation<UpdateQuestionTextRequest>();
        group.MapPatch("/update-images", UpdateQuestionImages)
            .WithRequestValidation<UpdateQuestionImagesRequest>();
        group.MapPatch("/update-answer-settings", UpdateQuestionAnswerSettings)
            .WithRequestValidation<UpdateQuestionAnswerSettingsRequest>();
        // group.MapDelete("/delete", DeleteVokiQuestion);
        // group.MapPatch("/move-up-in-order", MoveQuestionUpInOrder);
        // group.MapPatch("/move-down-in-order", MoveQuestionDownInOrder);


        group.MapPost("/upload-image", UploadVokiQuestionImage)
            .DisableAntiforgery();
    }

    private static async Task<IResult> GetVokiQuestionFullData(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiQuestionWithAnswersQuery, VokiQuestion> handler
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();

        GetVokiQuestionWithAnswersQuery query = new(vokiId, questionId);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (question) => Results.Json(
            VokiQuestionFullDataResponse.Create(question)
        ));
    }

    private static async Task<IResult> UpdateQuestionText(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<UpdateQuestionTextCommand, VokiQuestionText> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateQuestionTextRequest>();

        UpdateQuestionTextCommand command = new(id, questionId, request.ParsedText);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (text) => Results.Json(
            new { NewText = text.ToString() }
        ));
    }

    private static async Task<IResult> UpdateQuestionImages(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<UpdateQuestionImagesCommand, VokiQuestionImagesSet> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateQuestionImagesRequest>();

        UpdateQuestionImagesCommand command = new(id, questionId, request.ParsedImagesSet);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (imgsSet) => Results.Json(new {
            NewImages = imgsSet.Keys.Select(s => s.ToString()).ToArray()
        }));
    }

    private static async Task<IResult> UpdateQuestionAnswerSettings(
        CancellationToken ct, HttpContext httpContext,
        ICommandHandler<UpdateQuestionAnswerSettingsCommand, VokiQuestion> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateQuestionAnswerSettingsRequest>();

        UpdateQuestionAnswerSettingsCommand command = new(
            id, questionId, request.ParsedAnswersCountLimit, request.ShuffleAnswers
        );
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (question) => Results.Json(new {
            MinAnswers = question.AnswersCountLimit.MinAnswers,
            MaxAnswers = question.AnswersCountLimit.MaxAnswers,
            ShuffleAnswers = question.ShuffleAnswers
        }));
    }

    private static async Task<IResult> UploadVokiQuestionImage(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UploadVokiQuestionImageCommand, DraftGeneralVokiQuestionImageKey> handler,
        [FromForm] IFormFile file
    ) {
        VokiId vokiId = httpContext.GetVokiIdFromRoute();
        GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();

        UploadVokiQuestionImageCommand command = new(vokiId, questionId,
            new(file.OpenReadStream(), file.FileName, file.ContentType));
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (key) => Results.Json(
            new { ImageKey = key.ToString() }
        ));
    }
    // private static async Task<IResult> MoveQuestionUpInOrder(
    //     CancellationToken ct, HttpContext httpContext,
    //     ICommandHandler<MoveQuestionUpInOrderCommand, ImmutableDictionary<GeneralVokiQuestionId, ushort>> handler
    // ) {
    //     VokiId id = httpContext.GetVokiIdFromRoute();
    //     GeneralVokiQuestionId questionId = httpContext.GetQuestionIdFromRoute();
    //
    //
    //     MoveQuestionUpInOrderCommand command = new(id, questionId);
    //     var result = await handler.Handle(command, ct);
    //
    //     return CustomResults.FromErrOr(result, (questionsOrder) => Results.Json(
    //         new { Id = questionId.ToString() }
    //     ));
    // }
}