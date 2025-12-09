using ApplicationShared.messaging;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.temp_keys;

namespace VokiCreationServicesLib.Application.draft_vokis.commands;

public sealed record UpdateVokiCoverCommand(
    VokiId VokiId,
    TempImageKey CoverKey
) : ICommand<VokiCoverKey>,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiCoverCommandHandler : ICommandHandler<UpdateVokiCoverCommand, VokiCoverKey>
{
    private readonly IVokiCreationLibMainStorageBucket _vokiCreationLibMainStorageBucket;
    private readonly IDraftVokiRepository _draftVokiRepository;

    public UpdateVokiCoverCommandHandler(
        IVokiCreationLibMainStorageBucket vokiCreationLibMainStorageBucket,
        IDraftVokiRepository draftVokiRepository
    ) {
        _vokiCreationLibMainStorageBucket = vokiCreationLibMainStorageBucket;
        _draftVokiRepository = draftVokiRepository;
    }


    public async Task<ErrOr<VokiCoverKey>> Handle(UpdateVokiCoverCommand command, CancellationToken ct) {
        BaseDraftVoki voki = (await _draftVokiRepository.GetById(command.VokiId, ct))!;
        VokiCoverKey destination = VokiCoverKey.CreateWithId(command.VokiId, command.CoverKey.Extension);
        ErrOrNothing copyingRes = await _vokiCreationLibMainStorageBucket.CopyVokiCoverFromTempToStandard(
            command.CoverKey, destination, ct
        );
        if (copyingRes.IsErr(out var err)) {
            return err;
        }

        var coverUpdateRes = voki.UpdateCover(destination);
        if (coverUpdateRes.IsErr(out err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return voki.Cover;
    }
}