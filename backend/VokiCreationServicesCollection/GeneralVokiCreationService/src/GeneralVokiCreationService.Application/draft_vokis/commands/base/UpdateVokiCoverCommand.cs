using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Application.common.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.@base;

public sealed record UpdateVokiCoverCommand(
    VokiId VokiId,
    TempImageKey CoverKey
) : ICommand<VokiCoverKey>,   
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiCoverCommandHandler : ICommandHandler<UpdateVokiCoverCommand, VokiCoverKey>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public UpdateVokiCoverCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IMainStorageBucket mainStorageBucket
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<VokiCoverKey>> Handle(UpdateVokiCoverCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetById(command.VokiId))!;
        VokiCoverKey destination = VokiCoverKey.CreateWithId(command.VokiId, command.CoverKey.Extension);
        ErrOrNothing copyingRes = await _mainStorageBucket.CopyVokiCoverFromTempToStandard(
            command.CoverKey, destination, ct
        );
        if (copyingRes.IsErr(out var err)) {
            return err;
        }

        var coverUpdateRes = voki.UpdateCover(destination);
        if (coverUpdateRes.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki);
        return voki.Cover;
    }
}