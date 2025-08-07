using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record UpdateQuestionAnswerSettingsCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    QuestionAnswersCountLimit NewCountLimit,
    bool ShuffleAnswers
) :
    ICommand<VokiQuestion>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateQuestionAnswerSettingsCommandHandler :
    ICommandHandler<UpdateQuestionAnswerSettingsCommand, VokiQuestion>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public UpdateQuestionAnswerSettingsCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiQuestion>> Handle(UpdateQuestionAnswerSettingsCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestions(command.VokiId))!;
        var res = voki.UpdateQuestionAnswerSettings(
            command.QuestionId, command.NewCountLimit, command.ShuffleAnswers
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return res.AsSuccess();
    }
}