using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.common;

public interface IMainStorageBucket
{
    Task<ErrOrNothing> CopyDefaultVokiCoverForVoki(VokiCoverKey defaultVokiCover, CancellationToken ct);
    Task<ErrOrNothing> CopyVokiCoverFromTempToStandard(TempImageKey temp, VokiCoverKey destination, CancellationToken ct);

    Task<ErrOrNothing> CopyVokiResultImageFromTempToStandard(
        TempImageKey temp,
        GeneralVokiResultImageKey destination,
        CancellationToken ct
    );

    Task<ErrOrNothing> CopyVokiQuestionImagesFromTempToStandard(
        Dictionary<TempImageKey, GeneralVokiQuestionImageKey> tempToDestination,
        CancellationToken ct
    );

    Task<ErrOrNothing> CopyVokiAnswerImageFromTempToStandard(
        TempImageKey temp,
        GeneralVokiAnswerImageKey destination,
        CancellationToken ct
    );

    Task<ErrOrNothing> CopyVokiAnswerAudioFromTempToStandard(
        TempAudioKey temp,
        GeneralVokiAnswerAudioKey destination,
        CancellationToken ct
    );
}