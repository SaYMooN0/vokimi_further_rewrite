using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel;
using VokiCreationServicesLib.Application;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.publishing;

public record class PublishVokiWithNoIssuesCommand(VokiId VokiId) :
    ICommand<VokiSuccessfullyPublishedResult>,
    IWithAuthCheckStep,
    IWithVokiPrimaryAuthorValidationStep;

internal sealed class PublishVokiWithNoIssuesCommandHandler :
    ICommandHandler<PublishVokiWithNoIssuesCommand, VokiSuccessfullyPublishedResult>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PublishVokiWithNoIssuesCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<VokiSuccessfullyPublishedResult>> Handle(
        PublishVokiWithNoIssuesCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestionsAndResultsForUpdate(command.VokiId, ct))!;

        var publishingRes = voki.PublishWithNoIssues(_dateTimeProvider);
        if (publishingRes.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Delete(voki, ct);
        return new VokiSuccessfullyPublishedResult(voki.Id, voki.Cover, voki.Name);
    }
}