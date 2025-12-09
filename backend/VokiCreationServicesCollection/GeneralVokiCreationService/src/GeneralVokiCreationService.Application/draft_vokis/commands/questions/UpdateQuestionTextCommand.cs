using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record UpdateQuestionTextCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    VokiQuestionText NewQuestionText
) :
    ICommand<VokiQuestionText>,   
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class UpdateQuestionTextCommandHandler : ICommandHandler<UpdateQuestionTextCommand, VokiQuestionText>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public UpdateQuestionTextCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<VokiQuestionText>> Handle(UpdateQuestionTextCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestions(command.VokiId, ct))!;
        var res = voki.UpdateQuestionText(command.QuestionId, command.NewQuestionText);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return res.AsSuccess().Text;
    }
}