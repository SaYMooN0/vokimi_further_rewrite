using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions.answers;

public sealed record AddNewAnswerToVokiQuestionCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    BaseVokiAnswerTypeData AnswerData
) :
    ICommand<VokiQuestionAnswer>,
    IWithVokiAccessValidationStep;

internal sealed class AddNewAnswerToVokiQuestionCommandHandler :
    ICommandHandler<AddNewAnswerToVokiQuestionCommand, VokiQuestionAnswer>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public AddNewAnswerToVokiQuestionCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiQuestionAnswer>> Handle(
        AddNewAnswerToVokiQuestionCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestionAnswers(command.VokiId))!;
        var res = voki.AddNewAnswerToQuestion(command.QuestionId, command.AnswerData);
        if (res.IsErr(out var err)) {
            return err;
        }
        await _draftGeneralVokiRepository.Update(voki);
        return res.AsSuccess();
    }
}