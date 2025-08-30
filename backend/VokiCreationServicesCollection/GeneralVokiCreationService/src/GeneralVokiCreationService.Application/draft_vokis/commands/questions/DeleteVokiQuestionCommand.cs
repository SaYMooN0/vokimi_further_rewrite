using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record DeleteVokiQuestionCommand(VokiId VokiId, GeneralVokiQuestionId QuestionId) :
    ICommand<ImmutableArray<VokiQuestion>>,
    IWithVokiAccessValidationStep;

internal sealed class DeleteVokiQuestionCommandHandler :
    ICommandHandler<DeleteVokiQuestionCommand, ImmutableArray<VokiQuestion>>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public DeleteVokiQuestionCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }


    public async Task<ErrOr<ImmutableArray<VokiQuestion>>> Handle(
        DeleteVokiQuestionCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestions(command.VokiId))!;
        bool wasDeleted = voki.DeleteQuestion(command.QuestionId);
        if (wasDeleted) {
            await _draftGeneralVokiRepository.Update(voki);
        }

        return voki.Questions;
    }
}