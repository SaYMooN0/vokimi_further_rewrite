using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.answers;

public sealed record UpdateVokiQuestionAnswerCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    GeneralVokiAnswerId AnswerId,
    BaseVokiAnswerTypeData NewAnswerTypeData,
    ImmutableHashSet<GeneralVokiResultId> NewRelatedResultIds
) :
    ICommand<VokiQuestionAnswer>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiQuestionAnswerCommandHandler :
    ICommandHandler<UpdateVokiQuestionAnswerCommand, VokiQuestionAnswer>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public UpdateVokiQuestionAnswerCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiQuestionAnswer>> Handle(UpdateVokiQuestionAnswerCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestionAnswersAndResults(command.VokiId))!;
        var res = voki.UpdateQuestionAnswer(
            command.QuestionId, command.AnswerId,
            command.NewAnswerTypeData, command.NewRelatedResultIds
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return res.AsSuccess();
    }
}