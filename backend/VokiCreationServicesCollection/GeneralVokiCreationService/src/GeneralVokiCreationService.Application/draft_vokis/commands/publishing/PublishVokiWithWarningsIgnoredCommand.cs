using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel;
using VokiCreationServicesLib.Application;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.publishing;

public sealed record PublishVokiWithWarningsIgnoredCommand(VokiId VokiId) :
    ICommand<VokiSuccessfullyPublishedResult>,
    IWithAuthCheckStep,
    IWithVokiPrimaryAuthorValidationStep;

internal sealed class PublishVokiWithWarningsIgnoredCommandHandler :
    ICommandHandler<PublishVokiWithWarningsIgnoredCommand, VokiSuccessfullyPublishedResult>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    public PublishVokiWithWarningsIgnoredCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<VokiSuccessfullyPublishedResult>> Handle(
        PublishVokiWithWarningsIgnoredCommand command,
        CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestionsAndResultsForUpdate(command.VokiId, ct))!;
        var publishingRes = voki.PublishWithWarningsIgnored(_dateTimeProvider);
        if (publishingRes.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Delete(voki, ct);
        return new VokiSuccessfullyPublishedResult(voki.Id, voki.Cover, voki.Name);
    }
}