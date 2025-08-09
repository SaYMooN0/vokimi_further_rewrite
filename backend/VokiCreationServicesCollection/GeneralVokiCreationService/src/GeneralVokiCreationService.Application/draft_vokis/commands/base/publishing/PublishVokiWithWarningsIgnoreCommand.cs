using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.@base.publishing;

public record class PublishVokiWithWarningsIgnoreCommand(VokiId VokiId) :
    ICommand,
    IWithVokiPrimaryAuthorValidationStep;

internal sealed class PublishVokiWithWarningsIgnoreCommandHandler :
    ICommandHandler<PublishVokiWithWarningsIgnoreCommand>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMainStorageBucket _storageBucket;


    public PublishVokiWithWarningsIgnoreCommandHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository,
        IDateTimeProvider dateTimeProvider,
        IMainStorageBucket storageBucket
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _dateTimeProvider = dateTimeProvider;
        _storageBucket = storageBucket;
    }

    public async Task<ErrOrNothing> Handle(PublishVokiWithWarningsIgnoreCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestionAnswersAndResults(command.VokiId))!;
        var issues = voki.CheckForPublishingIssues();
        var publishingRes = voki.PublishWithWarningsIgnore(_dateTimeProvider);
        if (publishingRes.IsErr(out var err)) {
            return err;
        }

        var storageRes = await _storageBucket.CopyDraftVokiContentToPublished(voki.Id);
        if (storageRes.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return ErrOrNothing.Nothing;
    }
}