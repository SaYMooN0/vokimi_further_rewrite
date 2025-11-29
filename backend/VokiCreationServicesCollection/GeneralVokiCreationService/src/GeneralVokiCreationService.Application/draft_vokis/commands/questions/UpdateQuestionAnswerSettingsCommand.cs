using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common.repositories;
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
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class UpdateQuestionAnswerSettingsCommandHandler :
    ICommandHandler<UpdateQuestionAnswerSettingsCommand, VokiQuestion>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public UpdateQuestionAnswerSettingsCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    public async Task<ErrOr<VokiQuestion>> Handle(UpdateQuestionAnswerSettingsCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestions(command.VokiId))!;
        var res = voki.UpdateQuestionAnswerSettings(
            command.QuestionId, command.NewCountLimit, command.ShuffleAnswers
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki);
        return res.AsSuccess();
    }
}