using Amazon.S3;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using UserProfilesService.Application;
using VokimiStorageKeysLib.concrete_keys;

namespace UserProfilesService.Infrastructure.storage;

internal class MainStorageBucket : BaseMainS3Bucket, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        S3MainBucketConf s3MainBucketConf,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, s3MainBucketConf, logger) { }


    public async Task<ErrOr<UserProfilePicKey>> CopyUserProfilePicFromDefaults(AppUserId userId) {
        var newKey = UserProfilePicKey.CreateNewForUser(userId, CommonStorageItemKey.DefaultProfilePic.ImageExtension)
            .AsSuccess();

        ErrOrNothing res = await CopyStandardToStandard(
            source: CommonStorageItemKey.DefaultProfilePic,
            destination: newKey
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        return newKey;
    }
}