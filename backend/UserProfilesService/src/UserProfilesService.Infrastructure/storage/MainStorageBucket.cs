using Amazon.S3;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using UserProfilesService.Application;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.users;

namespace UserProfilesService.Infrastructure.storage;

internal class MainStorageBucket : StorageBucketAccessor, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        S3MainBucket s3MainBucket,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, s3MainBucket, logger) { }


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