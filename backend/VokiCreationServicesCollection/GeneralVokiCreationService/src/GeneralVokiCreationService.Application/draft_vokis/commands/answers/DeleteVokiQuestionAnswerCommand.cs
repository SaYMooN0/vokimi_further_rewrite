using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.answers;

public sealed record DeleteVokiQuestionAnswerCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    GeneralVokiAnswerId AnswerId
) :
    ICommand,
    IWithVokiAccessValidationStep;

internal sealed class DeleteVokiQuestionAnswerCommandHandler : ICommandHandler<DeleteVokiQuestionAnswerCommand>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public DeleteVokiQuestionAnswerCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }


    public async Task<ErrOrNothing> Handle(
        DeleteVokiQuestionAnswerCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestionAnswers(command.VokiId))!;
        bool wasDeleted = voki.DeleteQuestionAnswer(command.QuestionId, command.AnswerId);
        if (wasDeleted) {
            await _draftGeneralVokiRepository.Update(voki);
        }

        return ErrOrNothing.Nothing;
    }
}