using VokimiStorageKeysLib;
using VokimiStorageKeysLib.base_keys;

namespace GeneralVokiTakingService.Application;

public interface IMainStorageBucket
{
    public Task<ErrOrNothing> DeleteUnusedVokiKeys(VokiId vokiId, IEnumerable<BaseStorageKey> usedKeys);
}