using GeneralVokiCreationService.Api.contracts.voki;
using GeneralVokiCreationService.Application.draft_vokis.commands;
using GeneralVokiCreationService.Application.draft_vokis.commands.@base;
using GeneralVokiCreationService.Application.draft_vokis.commands.@base.publishing;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.common.vokis;
using SharedKernel.exceptions;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing_issues;
using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Api.endpoints;

internal static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/vokis/{vokiId}/");

        group.WithGroupAuthenticationRequired();

        group.MapGet("/main-info", GetVokiMainInfo);

        group.MapPatch("/set-cover-to-default", SetVokiCoverToDefault);
        group.MapPatch("/update-cover", UpdateVokiCover)
            .DisableAntiforgery();

        group.MapPatch("/update-name", UpdateVokiName)
            .WithRequestValidation<UpdateVokiNameRequest>();
        group.MapPatch("/update-details", UpdateVokiDetails)
            .WithRequestValidation<UpdateVokiDetailsRequest>();
        group.MapPatch("/update-tags", UpdateVokiTags)
            .WithRequestValidation<UpdateVokiTagsRequest>();

        group.MapPatch("/update-voki-taking-process-settings", UpdateVokiTakingProcessSettings)
            .WithRequestValidation<UpdateVokiTakingProcessSettingsRequest>();

        group.MapGet("/publishing-errors", CheckVokiForPublishingErrors);
        group.MapPost("/publish", PublishVoki);
        group.MapPost("/publish-with-warnings-ignore", PublishVokiWithWarningsIgnore);
    }

    private static async Task<IResult> GetVokiMainInfo(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiQuery, DraftGeneralVoki> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        GetVokiQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (voki) => Results.Json(
            VokiMainInfoResponse.Create(voki)
        ));
    }

    private static async Task<IResult> UpdateVokiName(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiNameCommand, VokiName> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateVokiNameRequest>();

        UpdateVokiNameCommand command = new(id, request.ParsedVokiName);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (name) => Results.Json(
            new { NewVokiName = name.ToString() }
        ));
    }

    private static async Task<IResult> SetVokiCoverToDefault(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<SetVokiCoverToDefaultCommand, DraftVokiCoverKey> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        SetVokiCoverToDefaultCommand command = new(id);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (key) => Results.Json(
            new { NewCover = key.ToString() }
        ));
    }

    private static async Task<IResult> UpdateVokiCover(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiCoverCommand, DraftVokiCoverKey> handler,
        [FromForm] IFormFile file
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        UpdateVokiCoverCommand command = new(id, new(file.OpenReadStream(), file.FileName, file.ContentType));
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (key) => Results.Json(
            new { NewCover = key.ToString() }
        ));
    }

    private static async Task<IResult> UpdateVokiDetails(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiDetailsCommand, VokiDetails> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateVokiDetailsRequest>();

        UpdateVokiDetailsCommand command = new(id, request.ParsedDetails);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, details =>
            Results.Json(VokiDetailsResponse.FromDetails(details))
        );
    }

    private static async Task<IResult> UpdateVokiTags(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiTagsCommand, VokiTagsSet> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateVokiTagsRequest>();

        UpdateVokiTagsCommand command = new(id, request.ParsedTags);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (tagsSet) => Results.Json(
            new { NewTags = tagsSet.Value.Select(t => t.ToString()).ToArray() }
        ));
    }

    private static async Task<IResult> UpdateVokiTakingProcessSettings(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiTakingProcessSettingsCommand, VokiTakingProcessSettings> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var request = httpContext.GetValidatedRequest<UpdateVokiTakingProcessSettingsRequest>();

        UpdateVokiTakingProcessSettingsCommand command = new(id, request.ParsedSettings);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (settings) =>
            Results.Json(settings)
        );
    }

    private static async Task<IResult> CheckVokiForPublishingErrors(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiPublishingIssuesQuery, ImmutableArray<VokiPublishingIssue>> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        GetVokiPublishingIssuesQuery query = new(id);
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOr(result, (issues) => Results.Json(
            VokiPublishingIssuesResponse.Create(issues)
        ));
    }

    private static async Task<IResult> PublishVoki(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<PublishVokiCommand, PublishVokiCommandResult> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        PublishVokiCommand command = new(id);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOr(result, (r) => r switch {
            PublishVokiCommandResult.Success => Results.Ok(),
            PublishVokiCommandResult.FailedToPublish fail =>
                Results.Json(VokiPublishingIssuesResponse.Create(fail.Issues)),
            _ => throw new ArgumentException("Unknown Voki result command")
        });
    }

    private static async Task<IResult> PublishVokiWithWarningsIgnore(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<PublishVokiWithWarningsIgnoreCommand> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        PublishVokiWithWarningsIgnoreCommand command = new(id);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }
}