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
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public SetVokiCoverToDefaultCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IMainStorageBucket mainStorageBucket
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<VokiCoverKey>> Handle(SetVokiCoverToDefaultCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetById(command.VokiId))!;
        ImageFileExtension ext = CommonStorageItemKey.DefaultVokiCover.ImageExtension;
        VokiCoverKey defaultVokiCover = VokiCoverKey.CreateWithId(command.VokiId, ext);
        var copyRes = await _mainStorageBucket.CopyDefaultVokiCoverForVoki(defaultVokiCover);
        if (copyRes.IsErr(out var err)) {
            return err;
        }

        voki.UpdateCover(defaultVokiCover);
        await _draftGeneralVokisRepository.Update(voki);
        return voki.Cover;
    }
}