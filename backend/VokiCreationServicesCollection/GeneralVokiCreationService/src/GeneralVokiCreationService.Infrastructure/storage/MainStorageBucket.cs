using Amazon.S3;
using GeneralVokiCreationService.Application;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using VokimiStorageKeysLib.concrete_keys;

namespace GeneralVokiCreationService.Infrastructure.storage;

internal class MainStorageBucket : BaseMainS3Bucket, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        S3MainBucketConf s3MainBucketConf,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, s3MainBucketConf, logger) { }
    public Task<ErrOrNothing> CopyDefaultVokiCoverForNewVoki(VokiCoverKey destination) => CopyStandardToStandard(
        source: CommonStorageItemKey.DefaultVokiCover,
        destination: destination
    );
}