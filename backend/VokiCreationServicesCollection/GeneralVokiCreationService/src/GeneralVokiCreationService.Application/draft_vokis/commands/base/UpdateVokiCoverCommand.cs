using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.voki_cover;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.@base;

public sealed record UpdateVokiCoverCommand(VokiId VokiId, FileData File) :
    ICommand<VokiCoverKey>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiCoverCommandHandler : ICommandHandler<UpdateVokiCoverCommand, VokiCoverKey>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public UpdateVokiCoverCommandHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository,
        IMainStorageBucket mainStorageBucket
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<VokiCoverKey>> Handle(UpdateVokiCoverCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetById(command.VokiId))!;
        var uploadingRes = await _mainStorageBucket.UploadDraftVokiCover(command.VokiId, command.File);
        if (uploadingRes.IsErr(out var err)) {
            return err;
        }

        var coverUpdateRes = voki.UpdateCover(uploadingRes.AsSuccess());
        if (coverUpdateRes.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return voki.Cover;
    }
}