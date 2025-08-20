namespace InfrastructureShared.Storage;

public interface IStorageService
{
    public Task<ErrOrNothing> CopyFromTempToStandard(TempImageKey tempKey, BaseStorageImageKey destinationKey);
    public Task<TempImageKey> PutTempImageFile(FileData data);

    // public Task<ErrOrNothing> Delete(BaseStorageKey storageKey);
}