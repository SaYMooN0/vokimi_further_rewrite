using Amazon.S3;
using GeneralVokiTakingService.Application;
using InfrastructureShared.Storage;
using InfrastructureShared.Storage.s3;
using Microsoft.Extensions.Logging;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.base_keys;

namespace GeneralVokiTakingService.Infrastructure.storage;

internal class MainStorageBucket : StorageBucketAccessor, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        S3MainBucketConf s3MainBucketConf,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, s3MainBucketConf, logger) { }


    public async Task<ErrOrNothing> DeleteUnusedVokiKeys(VokiId vokiId, IEnumerable<BaseStorageKey> usedKeys) {
        string prefix = $"{KeyConsts.VokisFolder}/{vokiId}/";
        var usedStringifiedKeys = usedKeys
            .Select(k => k.ToString())
            .ToImmutableHashSet();
        return await base.DeleteFilesWithoutSubfoldersAsync(prefix, usedStringifiedKeys);
    }
}