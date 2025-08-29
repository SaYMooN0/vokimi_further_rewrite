using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record MoveQuestionDownInOrderCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId
) :
    ICommand<ImmutableArray<VokiQuestion>>,
    IWithVokiAccessValidationStep;

internal sealed class MoveQuestionDownInOrderCommandHandler :
    ICommandHandler<MoveQuestionDownInOrderCommand, ImmutableArray<VokiQuestion>>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public MoveQuestionDownInOrderCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<ImmutableArray<VokiQuestion>>> Handle(MoveQuestionDownInOrderCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestions(command.VokiId))!;
        ErrOrNothing res = voki.MoveQuestionDownInOrder(command.QuestionId);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return voki.Questions;
    }
}