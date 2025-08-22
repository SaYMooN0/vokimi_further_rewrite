using VokimiStorageKeysLib;
using VokimiStorageKeysLib.concrete_keys;

namespace GeneralVokiCreationService.Application;

public interface IMainStorageBucket
{
    Task<ErrOrNothing> CopyDefaultVokiCoverForVoki(VokiCoverKey defaultVokiCover);

    public Task<ErrOr<VokiCoverKey>> UploadVokiCover(VokiId vokiId, FileData file);
    public Task<ErrOr<GeneralVokiQuestionImageKey>> UploadVokiQuestionImage(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        FileData file
    );

    public Task<ErrOrNothing> DeleteUnusedQuestionImages(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        IEnumerable<GeneralVokiQuestionImageKey> usedKeys
    );

    public Task<ErrOrNothing> DeleteUnusedResultImages(
        VokiId vokiId,
        GeneralVokiResultId resultId,
        GeneralVokiResultImageKey? currentKey
    );

    Task<ErrOr<GeneralVokiResultImageKey>> UploadVokiResultImage(
        VokiId vokiId,
        GeneralVokiResultId resultId,
        FileData file
    );

}