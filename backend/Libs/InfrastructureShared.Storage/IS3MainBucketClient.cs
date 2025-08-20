namespace InfrastructureShared.Storage;

public interface IS3MainBucketClient
{
    internal Task<ErrOrNothing> PutFile(BaseStorageKey key, FileData file);
    internal Task<ErrOrNothing> PutFile(ITempKey key, FileData file);
    internal Task<ErrOrNothing> CopyFile(BaseStorageKey source, BaseStorageKey destination);
    internal Task<ErrOrNothing> CopyFile(ITempKey source, BaseStorageKey destination);
    public Task<ErrOr<FileData>> GetFileAsync(string key);
}