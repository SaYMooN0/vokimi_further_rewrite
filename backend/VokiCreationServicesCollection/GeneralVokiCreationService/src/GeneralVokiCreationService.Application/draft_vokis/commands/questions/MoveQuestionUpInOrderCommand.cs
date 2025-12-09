using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record MoveQuestionUpInOrderCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId
) :
    ICommand<ImmutableArray<VokiQuestion>>,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class MoveQuestionUpInOrderCommandHandler :
    ICommandHandler<MoveQuestionUpInOrderCommand, ImmutableArray<VokiQuestion>>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public MoveQuestionUpInOrderCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<ImmutableArray<VokiQuestion>>> Handle(MoveQuestionUpInOrderCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestions(command.VokiId, ct))!;
        ErrOrNothing res = voki.MoveQuestionUpInOrder(command.QuestionId);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return voki.Questions;
    }
}