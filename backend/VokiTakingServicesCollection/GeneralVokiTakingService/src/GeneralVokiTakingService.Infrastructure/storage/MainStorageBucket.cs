using Amazon.S3;
using GeneralVokiTakingService.Application;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.voki_cover;

namespace GeneralVokiTakingService.Infrastructure.storage;

internal class MainStorageBucket : BaseStorageBucket, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        MainBucketNameProvider mainBucketNameProvider,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, mainBucketNameProvider, logger) { }


    public Task<ErrOr<VokiCoverKey>> DeleteUnusedVokiKeys(VokiId vokiId, IEnumerable<BaseStorageKey> usedKeys) =>
        throw new NotImplementedException("DeleteUnusedVokiKeys is not implemented");
}