using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.repositories;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Application.draft_vokis.commands;

public sealed record UpdateVokiCoverCommand(VokiId VokiId, DraftVokiCoverKey NewCover) :
    ICommand<DraftVokiCoverKey>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiCoverCommandHandler :
    ICommandHandler<UpdateVokiCoverCommand, DraftVokiCoverKey>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public UpdateVokiCoverCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<DraftVokiCoverKey>> Handle(UpdateVokiCoverCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetById(command.VokiId))!;
        var result = voki.UpdateCover(command.NewCover);
        if (result.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return voki.Cover;
    }
}