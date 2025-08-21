using Amazon.S3;
using InfrastructureShared.Storage;
using InfrastructureShared.Storage.s3;
using Microsoft.Extensions.Logging;
using UserProfilesService.Application;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.concrete_keys;

namespace UserProfilesService.Infrastructure.storage;

internal class MainStorageBucket : StorageBucketAccessor, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        S3MainBucketConf s3MainBucketConf,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, s3MainBucketConf, logger) { }


    public async Task<ErrOr<UserProfilePicKey>> CopyUserProfilePicFromDefaults(AppUserId userId) {
        var newKey = UserProfilePicKey.CreateNewForUser(userId, "webp").AsSuccess();

        ErrOrNothing res = await CopySingleObjectAsync(
            source: KeyConsts.DefaultUserProfilePic,
            destinationKey: newKey
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        return newKey;
    }
}