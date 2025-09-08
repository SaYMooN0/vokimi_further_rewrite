using GeneralVokiCreationService.Application.draft_vokis.commands.answers.auxiliary;
using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.answers;

public sealed record AddNewAnswerToVokiQuestionCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    VokiAnswerTypeDataDto AnswerDataDto,
    ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
) :
    ICommand<VokiQuestionAnswer>,
    IWithVokiAccessValidationStep;

internal sealed class AddNewAnswerToVokiQuestionCommandHandler :
    ICommandHandler<AddNewAnswerToVokiQuestionCommand, VokiQuestionAnswer>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly DraftVokiAnswerDataSavingService _draftVokiAnswerDataSavingService;

    public AddNewAnswerToVokiQuestionCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository,
        DraftVokiAnswerDataSavingService draftVokiAnswerDataSavingService) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _draftVokiAnswerDataSavingService = draftVokiAnswerDataSavingService;
    }

    public async Task<ErrOr<VokiQuestionAnswer>> Handle(
        AddNewAnswerToVokiQuestionCommand command, CancellationToken ct
    ) {
        ErrOr<BaseVokiAnswerTypeData> answerDataRes = await _draftVokiAnswerDataSavingService.SaveAnswerData(
            command.VokiId, command.QuestionId, command.AnswerDataDto
        );

        if (answerDataRes.IsErr(out var err)) {
            return err;
        }

        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestionAnswersAndResults(command.VokiId))!;

        ErrOr<VokiQuestionAnswer> res = voki.AddNewAnswerToQuestion(
            command.QuestionId, answerDataRes.AsSuccess(), command.RelatedResultIds
        );
        if (res.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki);
        return res.AsSuccess();
    }
}