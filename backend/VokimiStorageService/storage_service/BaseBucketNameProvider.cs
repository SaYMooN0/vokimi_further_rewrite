namespace VokimiStorageService.storage_service;

public abstract class BaseBucketNameProvider
{
    public string BucketName { get; }

    protected BaseBucketNameProvider(string bucketName) {
        BucketName = bucketName;
    }
}