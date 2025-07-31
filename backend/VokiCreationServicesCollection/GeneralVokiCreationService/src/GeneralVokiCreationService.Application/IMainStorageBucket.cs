using VokimiStorageKeysLib;
using VokimiStorageKeysLib.draft_general_voki.question_image;
using VokimiStorageKeysLib.draft_general_voki.result_image;
using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Application;

public interface IMainStorageBucket
{
    public Task<ErrOr<DraftVokiCoverKey>> UploadDraftVokiCover(VokiId vokiId, FileData file);

    Task<ErrOrNothing> DeleteVokiCover(DraftVokiCoverKey key);

    public Task<ErrOr<DraftGeneralVokiQuestionImageKey>> UploadVokiQuestionImage(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        FileData file
    );

    public Task<ErrOrNothing> DeleteUnusedQuestionImages(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        IEnumerable<DraftGeneralVokiQuestionImageKey> usedKeys
    );

    Task<ErrOr<DraftGeneralVokiResultImageKey>> UploadVokiResultImage(
        VokiId vokiId,
        GeneralVokiResultId resultId,
        FileData file
    );
}