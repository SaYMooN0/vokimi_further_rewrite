using Amazon.S3;

namespace VokimiStorageService.storage_service.buckets;

public class MainStorageBucket : BaseStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        MainBucketNameProvider mainBucketNameProvider,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, mainBucketNameProvider, logger) { }
}

public class MainBucketNameProvider : BaseBucketNameProvider
{
    public MainBucketNameProvider(string bucketName) : base(bucketName) { }
}