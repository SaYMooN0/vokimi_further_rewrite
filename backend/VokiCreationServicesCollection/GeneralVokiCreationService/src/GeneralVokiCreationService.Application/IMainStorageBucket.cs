using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application;

public interface IMainStorageBucket
{
    Task<ErrOrNothing> CopyDefaultVokiCoverForVoki(
        VokiCoverKey defaultVokiCover
    );

    public Task<ErrOrNothing> CopyVokiCoverFromTempToStandard(
        TempImageKey temp,
        VokiCoverKey destination
    );

    public Task<ErrOrNothing> CopyVokiResultImageFromTempToStandard(
        TempImageKey temp,
        GeneralVokiResultImageKey destination
    );

    public Task<ErrOrNothing> CopyVokiQuestionImageFromTempToStandard(
        TempImageKey temp,
        GeneralVokiQuestionImageKey destination
    );
}