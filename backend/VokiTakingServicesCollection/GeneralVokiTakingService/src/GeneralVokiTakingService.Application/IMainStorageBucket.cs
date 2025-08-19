using VokimiStorageKeysLib;

namespace GeneralVokiTakingService.Application;

public interface IMainStorageBucket
{
    public Task<ErrOrNothing> DeleteUnusedVokiKeys(VokiId vokiId, IEnumerable<BaseStorageKey> usedKeys);
}