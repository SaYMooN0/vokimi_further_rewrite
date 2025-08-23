using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.extension;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.@base;

public sealed record SetVokiCoverToDefaultCommand(VokiId VokiId) :
    ICommand<VokiCoverKey>,
    IWithVokiAccessValidationStep;

internal sealed class SetVokiCoverToDefaultCommandHandler :
    ICommandHandler<SetVokiCoverToDefaultCommand, VokiCoverKey>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public SetVokiCoverToDefaultCommandHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository,
        IMainStorageBucket mainStorageBucket
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<VokiCoverKey>> Handle(SetVokiCoverToDefaultCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetById(command.VokiId))!;
        ImageFileExtension ext = CommonStorageItemKey.DefaultVokiCover.ImageExtension;
        VokiCoverKey defaultVokiCover = VokiCoverKey.CreateWithId(command.VokiId, ext);
        var copyRes = await _mainStorageBucket.CopyDefaultVokiCoverForVoki(defaultVokiCover);
        if (copyRes.IsErr(out var err)) {
            return err;
        }

        voki.UpdateCover(defaultVokiCover);
        await _draftGeneralVokiRepository.Update(voki);
        return voki.Cover;
    }
}