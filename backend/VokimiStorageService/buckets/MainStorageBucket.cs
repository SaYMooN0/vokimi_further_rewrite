using Amazon.S3;
using InfrastructureShared.Storage;
using SharedKernel.errs;

namespace VokimiStorageService.buckets;

public class MainStorageBucket : BaseStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        MainBucketNameProvider mainBucketNameProvider,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, mainBucketNameProvider, logger) { }

    public new Task<ErrOr<(Stream Stream, string ContentType)>> GetFileAsync(string key) => base.GetFileAsync(key);
}

public class MainBucketNameProvider : BaseBucketNameProvider
{
    public MainBucketNameProvider(string bucketName) : base(bucketName) { }
}