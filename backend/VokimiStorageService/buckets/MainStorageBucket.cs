using Amazon.S3;
using InfrastructureShared.Storage;
using SharedKernel.errs;

namespace VokimiStorageService.buckets;

public class MainStorageBucket : StorageBucketAccessor
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        S3MainBucket s3MainBucket,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, s3MainBucket, logger) { }

    public new Task<ErrOr<(Stream Stream, string ContentType)>> GetFileAsync(string key) => base.GetFileAsync(key);
}
