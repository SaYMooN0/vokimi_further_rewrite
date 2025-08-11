using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.@base.publishing;

public sealed record  PublishVokiWithWarningsIgnoredCommand(VokiId VokiId) :
    ICommand<VokiId>,
    IWithVokiPrimaryAuthorValidationStep;

internal sealed class PublishVokiWithWarningsIgnoredCommandHandler :
    ICommandHandler<PublishVokiWithWarningsIgnoredCommand, VokiId>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly IDateTimeProvider _dateTimeProvider;


    public PublishVokiWithWarningsIgnoredCommandHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<VokiId>> Handle(PublishVokiWithWarningsIgnoredCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestionAnswersAndResults(command.VokiId))!;
        var publishingRes = voki.PublishWithWarningsIgnored(_dateTimeProvider);
        if (publishingRes.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return voki.Id;
    }
}