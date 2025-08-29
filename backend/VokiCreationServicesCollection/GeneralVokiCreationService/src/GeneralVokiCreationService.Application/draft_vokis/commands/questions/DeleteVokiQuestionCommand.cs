using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record DeleteVokiQuestionCommand(VokiId VokiId, GeneralVokiQuestionId QuestionId) :
    ICommand,
    IWithVokiAccessValidationStep;

internal sealed class DeleteVokiQuestionCommandHandler : ICommandHandler<DeleteVokiQuestionCommand>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public DeleteVokiQuestionCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }


    public async Task<ErrOrNothing> Handle(
        DeleteVokiQuestionCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestions(command.VokiId))!;
        bool wasDeleted = voki.DeleteQuestion(command.QuestionId);
        if (wasDeleted) {
            await _draftGeneralVokiRepository.Update(voki);
        }
        return ErrOrNothing.Nothing;
    }
}