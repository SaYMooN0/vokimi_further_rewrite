using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Application;

public interface IMainStorageBucket
{
    public Task<ErrOr<DraftVokiCoverKey>> UploadDraftVokiCover(
        VokiId vokiId,
        Stream fileStream,
        string fileName,
        string fileContentType
    );

    Task DeleteVokiCover(DraftVokiCoverKey key);
}