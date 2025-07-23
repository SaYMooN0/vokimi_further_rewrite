using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record UpdateQuestionImagesCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    VokiQuestionImagesSet NewImagesSet
) :
    ICommand<VokiQuestionImagesSet>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateQuestionImagesCommandHandler : ICommandHandler<UpdateQuestionImagesCommand, VokiQuestionImagesSet>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public UpdateQuestionImagesCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiQuestionImagesSet>> Handle(UpdateQuestionImagesCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetById(command.VokiId))!;
        var res = voki.UpdateQuestionImages(command.QuestionId, command.NewImagesSet);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return res.AsSuccess().Images;
    }
}