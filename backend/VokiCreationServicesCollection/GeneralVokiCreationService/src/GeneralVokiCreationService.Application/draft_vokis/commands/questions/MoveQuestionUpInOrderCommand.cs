using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record MoveQuestionUpInOrderCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId
) :
    ICommand<ImmutableArray<VokiQuestion>>,
    IWithVokiAccessValidationStep;

internal sealed class MoveQuestionUpInOrderCommandHandler :
    ICommandHandler<MoveQuestionUpInOrderCommand, ImmutableArray<VokiQuestion>>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public MoveQuestionUpInOrderCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<ImmutableArray<VokiQuestion>>> Handle(MoveQuestionUpInOrderCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestions(command.VokiId))!;
        ErrOrNothing res = voki.MoveQuestionUpInOrder(command.QuestionId);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return voki.Questions;
    }
}