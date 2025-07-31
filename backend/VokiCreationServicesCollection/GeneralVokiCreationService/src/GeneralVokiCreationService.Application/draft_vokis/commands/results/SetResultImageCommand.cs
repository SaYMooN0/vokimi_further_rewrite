using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.draft_general_voki.result_image;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record SetResultImageCommand(VokiId VokiId, GeneralVokiResultId ResultId, FileData File) :
    ICommand<DraftGeneralVokiResultImageKey>,
    IWithVokiAccessValidationStep;

internal sealed class
    SetResultImageCommandHandler : ICommandHandler<SetResultImageCommand, DraftGeneralVokiResultImageKey>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public SetResultImageCommandHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository,
        IMainStorageBucket mainStorageBucket
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<DraftGeneralVokiResultImageKey>> Handle(
        SetResultImageCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithResults(command.VokiId))!;
        var uploadingRes = await _mainStorageBucket.UploadVokiResultImage(
            command.VokiId, command.ResultId, command.File
        );
        if (uploadingRes.IsErr(out var err)) {
            return err;
        }

        var updateRes = voki.SetResultImage(command.ResultId, uploadingRes.AsSuccess());
        if (updateRes.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return updateRes.AsSuccess().Image!;
    }
}