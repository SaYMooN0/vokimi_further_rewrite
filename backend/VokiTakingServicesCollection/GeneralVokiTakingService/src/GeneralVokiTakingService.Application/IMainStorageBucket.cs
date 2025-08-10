using VokimiStorageKeysLib;
using VokimiStorageKeysLib.voki_cover;

namespace GeneralVokiTakingService.Application;

public interface IMainStorageBucket
{
    public Task<ErrOr<VokiCoverKey>> DeleteUnusedVokiKeys(VokiId vokiId, IEnumerable<BaseStorageKey> usedKeys);
}