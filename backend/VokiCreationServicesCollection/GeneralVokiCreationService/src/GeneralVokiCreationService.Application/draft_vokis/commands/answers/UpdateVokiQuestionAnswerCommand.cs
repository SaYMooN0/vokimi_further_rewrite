using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Application.draft_vokis.commands.answers.auxiliary;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.answers;

public sealed record UpdateVokiQuestionAnswerCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    GeneralVokiAnswerId AnswerId,
    VokiAnswerTypeDataDto AnswerDataDto,
    ImmutableHashSet<GeneralVokiResultId> NewRelatedResultIds
) :
    ICommand<VokiQuestionAnswer>,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiQuestionAnswerCommandHandler :
    ICommandHandler<UpdateVokiQuestionAnswerCommand, VokiQuestionAnswer>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly DraftVokiAnswerDataSavingService _draftVokiAnswerDataSavingService;

    public UpdateVokiQuestionAnswerCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        DraftVokiAnswerDataSavingService draftVokiAnswerDataSavingService
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _draftVokiAnswerDataSavingService = draftVokiAnswerDataSavingService;
    }

    public async Task<ErrOr<VokiQuestionAnswer>> Handle(UpdateVokiQuestionAnswerCommand command, CancellationToken ct) {
        ErrOr<BaseVokiAnswerTypeData> answerDataRes = await _draftVokiAnswerDataSavingService.SaveAnswerData(
            command.VokiId, command.QuestionId, command.AnswerDataDto, ct
        );

        if (answerDataRes.IsErr(out var err)) {
            return err;
        }

        DraftGeneralVoki voki =
            (await _draftGeneralVokisRepository.GetWithQuestionAnswersAndResults(command.VokiId, ct))!;
        ErrOr<VokiQuestionAnswer> res = voki.UpdateQuestionAnswer(
            command.QuestionId, command.AnswerId, answerDataRes.AsSuccess(), command.NewRelatedResultIds
        );
        if (res.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki, ct);
        return res.AsSuccess();
    }
}