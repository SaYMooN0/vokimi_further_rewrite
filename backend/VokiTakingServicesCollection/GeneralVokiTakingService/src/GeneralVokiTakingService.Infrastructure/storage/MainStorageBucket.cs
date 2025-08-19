using Amazon.S3;
using GeneralVokiTakingService.Application;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using VokimiStorageKeysLib;

namespace GeneralVokiTakingService.Infrastructure.storage;

internal class MainStorageBucket : StorageBucketAccessor, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        S3MainBucket s3MainBucket,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, s3MainBucket, logger) { }


    public async Task<ErrOrNothing> DeleteUnusedVokiKeys(VokiId vokiId, IEnumerable<BaseStorageKey> usedKeys) {
        string prefix = $"{KeyConsts.VokisFolder}/{vokiId}/";
        var usedStringifiedKeys = usedKeys
            .Select(k => k.ToString())
            .ToImmutableHashSet();
        return await base.DeleteFilesWithoutSubfoldersAsync(prefix, usedStringifiedKeys);
    }
}