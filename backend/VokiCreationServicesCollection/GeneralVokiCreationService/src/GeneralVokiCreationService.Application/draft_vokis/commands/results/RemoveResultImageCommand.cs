using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record RemoveResultImageCommand(VokiId VokiId, GeneralVokiResultId ResultId) :
    ICommand,
    IWithVokiAccessValidationStep;

internal sealed class RemoveResultImageCommandHandler :
    ICommandHandler<RemoveResultImageCommand>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public RemoveResultImageCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOrNothing> Handle(RemoveResultImageCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithResults(command.VokiId))!;

        var res = voki.RemoveResultImage(command.ResultId);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return ErrOrNothing.Nothing;
    }
}