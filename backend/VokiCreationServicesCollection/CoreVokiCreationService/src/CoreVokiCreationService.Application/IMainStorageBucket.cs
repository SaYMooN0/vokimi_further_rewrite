using VokimiStorageKeysLib.concrete_keys;

namespace CoreVokiCreationService.Application;

public interface IMainStorageBucket
{
    public Task<ErrOrNothing> CopyDefaultVokiCoverForNewVoki(VokiCoverKey destination);
}