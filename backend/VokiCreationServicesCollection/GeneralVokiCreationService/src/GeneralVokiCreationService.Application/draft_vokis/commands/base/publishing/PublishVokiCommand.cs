﻿using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel;
using VokiCreationServicesLib.Application;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.@base.publishing;

public record class PublishVokiCommand(VokiId VokiId) :
    ICommand<PublishVokiCommandResult>,
    IWithVokiPrimaryAuthorValidationStep;

public abstract record PublishVokiCommandResult
{
    public sealed record Success(VokiSuccessfullyPublishedResult VokiData) : PublishVokiCommandResult;

    public sealed record FailedToPublish(ImmutableArray<VokiPublishingIssue> Issues) : PublishVokiCommandResult;
}

internal sealed class PublishVokiCommandHandler :
    ICommandHandler<PublishVokiCommand, PublishVokiCommandResult>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly IDateTimeProvider _dateTimeProvider;


    public PublishVokiCommandHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<PublishVokiCommandResult>> Handle(PublishVokiCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestionAnswersAndResults(command.VokiId))!;
        var issues = voki.CheckForPublishingIssues();
        if (issues.Any()) {
            return new PublishVokiCommandResult.FailedToPublish(issues);
        }

        var publishingRes = voki.PublishWithWarningsIgnored(_dateTimeProvider);
        if (publishingRes.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Delete(voki);
        return new PublishVokiCommandResult.Success(
            new VokiSuccessfullyPublishedResult(voki.Id, voki.Cover, voki.Name)
        );
    }
}