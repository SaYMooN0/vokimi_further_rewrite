using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.answers;

public sealed record DeleteVokiQuestionAnswerCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    GeneralVokiAnswerId AnswerId
) :
    ICommand,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class DeleteVokiQuestionAnswerCommandHandler : ICommandHandler<DeleteVokiQuestionAnswerCommand>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public DeleteVokiQuestionAnswerCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }


    public async Task<ErrOrNothing> Handle(
        DeleteVokiQuestionAnswerCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestionAnswers(command.VokiId))!;
        bool wasDeleted = voki.DeleteQuestionAnswer(command.QuestionId, command.AnswerId);
        if (wasDeleted) {
            await _draftGeneralVokisRepository.Update(voki);
        }

        return ErrOrNothing.Nothing;
    }
}