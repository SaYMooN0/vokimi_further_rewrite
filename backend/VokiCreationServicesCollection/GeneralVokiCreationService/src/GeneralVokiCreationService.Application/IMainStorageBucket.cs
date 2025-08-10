using VokimiStorageKeysLib;
using VokimiStorageKeysLib.general_voki.question_image;
using VokimiStorageKeysLib.general_voki.result_image;
using VokimiStorageKeysLib.voki_cover;

namespace GeneralVokiCreationService.Application;

public interface IMainStorageBucket
{
    public Task<ErrOr<VokiCoverKey>> UploadDraftVokiCover(VokiId vokiId, FileData file);

    Task<ErrOrNothing> DeleteVokiCover(VokiCoverKey key);

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