namespace VokimiStorageService.s3_storage.s3;

internal interface IS3MainBucketClient
{
    internal Task<ErrOrNothing> PutFile(ITempKey key, FileData file, CancellationToken ct = default);
    public Task<ErrOr<FileData>> GetFile(string key, CancellationToken ct = default);
}