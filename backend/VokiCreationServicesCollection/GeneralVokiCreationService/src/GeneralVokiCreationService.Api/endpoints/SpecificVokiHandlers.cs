using GeneralVokiCreationService.Api.contracts;
using GeneralVokiCreationService.Application.draft_vokis.commands;
using GeneralVokiCreationService.Application.draft_vokis.commands.publishing;
using GeneralVokiCreationService.Application.draft_vokis.queries;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel.common.vokis.general_vokis;
using VokiCreationServicesLib.Api;
using VokiCreationServicesLib.Api.contracts;
using VokiCreationServicesLib.Application;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;

namespace GeneralVokiCreationService.Api.endpoints;

internal class SpecificVokiHandlers : BaseSpecificVokiHandlers, IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder) {
        var group = base.CreateGroupWithBaseEndpoint(routeBuilder);

        group.MapGet("/main-info", GetVokiMainInfo);

        group.MapPatch("/update-interaction-settings", UpdateVokiInteractionSettings)
            .WithRequestValidation<UpdateInteractionSettingsRequest>();

        group.MapPatch("/update-voki-taking-process-settings", UpdateVokiTakingProcessSettings)
            .WithRequestValidation<UpdateVokiTakingProcessSettingsRequest>();
    }


    private static async Task<IResult> GetVokiMainInfo(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiQuery, DraftGeneralVoki> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();

        var result = await handler.Handle(new GetVokiQuery(id), ct);

        return CustomResults.FromErrOrToJson<DraftGeneralVoki, VokiMainInfoResponse>(result);
    }

    private static async Task<IResult> UpdateVokiInteractionSettings(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiInteractionSettingsCommand, GeneralVokiInteractionSettings> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var req = httpContext.GetValidatedRequest<UpdateInteractionSettingsRequest>();

        var result = await handler.Handle(new UpdateVokiInteractionSettingsCommand(id, req.ParsedSettings), ct);
        return CustomResults.FromErrOrToJson<GeneralVokiInteractionSettings, VokiInteractionSettingsResponse>(result);
    }

    private static async Task<IResult> UpdateVokiTakingProcessSettings(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<UpdateVokiTakingProcessSettingsCommand, VokiTakingProcessSettings> handler
    ) {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var req = httpContext.GetValidatedRequest<UpdateVokiTakingProcessSettingsRequest>();

        var result = await handler.Handle(new UpdateVokiTakingProcessSettingsCommand(id, req.ParsedSettings), ct);
        return CustomResults.FromErrOr(result, settings => Results.Json(settings));
    }

    protected override Delegate CheckVokiForPublishingIssuesHandler => async (
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetVokiPublishingIssuesQuery, ImmutableArray<VokiPublishingIssue>> handler
    ) => {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var result = await handler.Handle(new GetVokiPublishingIssuesQuery(id), ct);

        return CustomResults.FromErrOr(result, issues =>
            Results.Json(new { Issues = issues.Select(VokiPublishingIssueResponse.Create).ToArray() }));
    };

    protected override Delegate PublishVokiHandler => async (
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<PublishVokiCommand, PublishVokiCommandResult> handler
    ) => {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var result = await handler.Handle(new PublishVokiCommand(id), ct);

        return CustomResults.FromErrOr(result, r => r switch {
            PublishVokiCommandResult.Success s =>
                Results.Json(VokiSuccessfullyPublishedResponse.Create(s.VokiData)),

            PublishVokiCommandResult.FailedToPublish f =>
                Results.Json(new { Issues = f.Issues.Select(VokiPublishingIssueResponse.Create).ToArray() }),

            _ => throw new ArgumentException("Unknown publish result")
        });
    };

    protected override Delegate PublishVokiWithWarningsIgnoredHandler => async (
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<PublishVokiWithWarningsIgnoredCommand, VokiSuccessfullyPublishedResult> handler
    ) => {
        VokiId id = httpContext.GetVokiIdFromRoute();
        var result = await handler.Handle(new PublishVokiWithWarningsIgnoredCommand(id), ct);

        return CustomResults.FromErrOrToJson<
            VokiSuccessfullyPublishedResult,
            VokiSuccessfullyPublishedResponse
        >(result);
    };
}