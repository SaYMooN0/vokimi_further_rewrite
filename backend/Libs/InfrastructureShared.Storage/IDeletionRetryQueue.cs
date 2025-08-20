using VokimiStorageKeysLib.base_keys;

namespace InfrastructureShared.Storage;

public interface IDeletionRetryQueue
{
    public Task Add(BaseStorageKey key);
    public Task AddRange(BaseStorageKey key);
}