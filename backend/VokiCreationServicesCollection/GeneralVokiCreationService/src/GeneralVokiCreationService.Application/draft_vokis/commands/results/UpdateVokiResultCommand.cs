using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record UpdateVokiResultCommand(
    VokiId VokiId,
    GeneralVokiResultId ResultId,
    VokiResultName NewName,
    VokiResultText NewText,
    string? NewImage
) :
    ICommand<VokiResult>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateResultTextCommandHandler : ICommandHandler<UpdateVokiResultCommand, VokiResult>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public UpdateResultTextCommandHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository,
        IMainStorageBucket mainStorageBucket
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<VokiResult>> Handle(UpdateVokiResultCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithResults(command.VokiId))!;
        var imageRes = await HandleImage(command.NewImage, command.VokiId, command.ResultId);
        if (imageRes.IsErr(out var err)) {
            return err;
        }

        var res = voki.UpdateResult(command.ResultId, command.NewName, command.NewText, imageRes.AsSuccess());
        if (res.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return res.AsSuccess();
    }

    private async Task<ErrOr<GeneralVokiResultImageKey?>> HandleImage(
        string? resultImageKey,
        VokiId vokiId,
        GeneralVokiResultId resultId
    ) {
        if (resultImageKey is null) {
            return ErrOr<GeneralVokiResultImageKey?>.Success(null);
        }

        if (resultImageKey.StartsWith(KeyConsts.TempFolder)) {
            return await HandleTempKey(resultImageKey, vokiId, resultId);
        }

        var creationRes = GeneralVokiResultImageKey.FromString(resultImageKey);
        if (creationRes.IsErr(out var err)) {
            return err;
        }

        var savedKey = creationRes.AsSuccess();
        if (!savedKey.IsWithIds(vokiId, resultId)) {
            return ErrFactory.Conflict("Specified result image key does not belong to this Voki result");
        }

        return savedKey;
    }

    private async Task<ErrOr<GeneralVokiResultImageKey?>> HandleTempKey(
        string stringTempKey, VokiId vokiId, GeneralVokiResultId resultId
    ) {
        var creationRes = TempImageKey.FromString(stringTempKey);
        if (creationRes.IsErr(out var creationErr)) {
            return creationErr;
        }

        var tempKey = creationRes.AsSuccess();
        var savedImageKey = GeneralVokiResultImageKey.CreateForResult(vokiId, resultId, tempKey.Extension);
        var copyingRes = await _mainStorageBucket.CopyVokiResultImageFromTempToStandard(
            tempKey, savedImageKey
        );
        if (copyingRes.IsErr(out var err)) {
            return err;
        }

        return savedImageKey;
    }
}