using GeneralVokiCreationService.Domain.common.interfaces.repositories;
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
    IWithVokiAccessValidationStep;

internal sealed class UpdateQuestionTextCommandHandler : ICommandHandler<UpdateQuestionTextCommand, VokiQuestionText>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public UpdateQuestionTextCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiQuestionText>> Handle(UpdateQuestionTextCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestions(command.VokiId))!;
        var res = voki.UpdateQuestionText(command.QuestionId, command.NewQuestionText);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return res.AsSuccess().Text;
    }
}