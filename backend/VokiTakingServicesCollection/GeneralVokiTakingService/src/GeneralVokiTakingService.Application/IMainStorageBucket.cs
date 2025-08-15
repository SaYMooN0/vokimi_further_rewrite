using VokimiStorageKeysLib;
using VokimiStorageKeysLib.voki_cover;

namespace GeneralVokiTakingService.Application;

public interface IMainStorageBucket
{
    public Task<ErrOrNothing> DeleteUnusedVokiKeys(VokiId vokiId, IEnumerable<BaseStorageKey> usedKeys);
}