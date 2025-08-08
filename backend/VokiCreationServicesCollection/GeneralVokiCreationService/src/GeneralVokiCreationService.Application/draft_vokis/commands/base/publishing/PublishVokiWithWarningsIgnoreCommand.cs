using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.@base.publishing;

public record class PublishVokiWithWarningsIgnoreCommand(VokiId VokiId) :
    ICommand,
    IWithVokiPrimaryAuthorValidationStep;


internal sealed class PublishVokiWithWarningsIgnoreCommandHandler :
    ICommandHandler<PublishVokiWithWarningsIgnoreCommand>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public PublishVokiWithWarningsIgnoreCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOrNothing> Handle(PublishVokiWithWarningsIgnoreCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestionAnswersAndResults(command.VokiId))!;
        var issues = voki.CheckForPublishingIssues();
        var publishingRes = voki.PublishWithWarningsIgnore();
        if (publishingRes.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return ErrOrNothing.Nothing;
    }
}