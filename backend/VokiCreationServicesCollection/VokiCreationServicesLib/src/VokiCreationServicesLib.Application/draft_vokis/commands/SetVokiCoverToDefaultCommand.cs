using ApplicationShared.messaging;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.extension;

namespace VokiCreationServicesLib.Application.draft_vokis.commands;

public sealed record SetVokiCoverToDefaultCommand(VokiId VokiId) :
    ICommand<VokiCoverKey>,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class SetVokiCoverToDefaultCommandHandler : ICommandHandler<SetVokiCoverToDefaultCommand, VokiCoverKey>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    private readonly IVokiCreationLibMainStorageBucket _libMainStorageBucket;

    public SetVokiCoverToDefaultCommandHandler(
        IDraftVokiRepository draftVokiRepository,
        IVokiCreationLibMainStorageBucket libMainStorageBucket
    ) {
        _draftVokiRepository = draftVokiRepository;
        _libMainStorageBucket = libMainStorageBucket;
    }


    public async Task<ErrOr<VokiCoverKey>> Handle(SetVokiCoverToDefaultCommand command, CancellationToken ct) {
        BaseDraftVoki voki = (await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct))!;
        ImageFileExtension ext = CommonStorageItemKey.DefaultVokiCover.ImageExtension;
        VokiCoverKey defaultVokiCover = VokiCoverKey.CreateWithId(command.VokiId, ext);
        ErrOrNothing copyRes = await _libMainStorageBucket.CopyDefaultVokiCoverForVoki(defaultVokiCover, ct);
        if (copyRes.IsErr(out var err)) {
            return err;
        }

        voki.UpdateCover(defaultVokiCover);
        await _draftVokiRepository.Update(voki, ct);
        return voki.Cover;
    }
}