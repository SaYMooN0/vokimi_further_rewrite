using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.common;

public interface IMainStorageBucket
{
    Task<ErrOrNothing> CopyDefaultVokiCoverForVoki(VokiCoverKey defaultVokiCover);
    Task<ErrOrNothing> CopyVokiCoverFromTempToStandard(TempImageKey temp, VokiCoverKey destination);
    Task<ErrOrNothing> CopyVokiResultImageFromTempToStandard(TempImageKey temp, GeneralVokiResultImageKey destination);

    Task<ErrOrNothing> CopyVokiQuestionImageFromTempToStandard(TempImageKey temp, GeneralVokiQuestionImageKey destination);

    Task<ErrOrNothing> CopyVokiAnswerImageFromTempToStandard(TempImageKey temp, GeneralVokiAnswerImageKey destination);
    Task<ErrOrNothing> CopyVokiAnswerAudioFromTempToStandard(TempAudioKey temp, GeneralVokiAnswerAudioKey destination);
}