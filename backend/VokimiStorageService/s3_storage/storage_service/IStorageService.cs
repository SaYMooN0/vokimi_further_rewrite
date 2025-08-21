namespace InfrastructureShared.Storage.storage_service;

public interface IStorageService
{
    public Task<ErrOr<TempImageKey>> PutTempImageFile(FileData data, CancellationToken ct = default);

    // public Task<ErrOrNothing> Delete(BaseStorageKey storageKey);
}