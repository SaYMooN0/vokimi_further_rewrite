using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record DeleteVokiResultCommand(VokiId VokiId, GeneralVokiResultId ResultId) :
    ICommand,
    IWithVokiAccessValidationStep;

internal sealed class DeleteVokiResultCommandHandler : ICommandHandler<DeleteVokiResultCommand>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public DeleteVokiResultCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }


    public async Task<ErrOrNothing> Handle(
        DeleteVokiResultCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestionAnswersAndResults(command.VokiId))!;
        bool wasDeleted = voki.DeleteResult(command.ResultId);
        if (wasDeleted) {
            await _draftGeneralVokiRepository.Update(voki);
        }
        return ErrOrNothing.Nothing;
    }
}