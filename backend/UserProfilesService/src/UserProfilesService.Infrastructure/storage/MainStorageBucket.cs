using Amazon.S3;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using UserProfilesService.Application;
using VokimiStorageKeysLib.users;

namespace UserProfilesService.Infrastructure.storage;

internal class MainStorageBucket : BaseStorageBucket, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        MainBucketNameProvider mainBucketNameProvider,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, mainBucketNameProvider, logger) { }

    private const string DefaultProfilePic = "/common/default-user-profile-pic.webp";

    public async Task<ErrOr<UserProfilePicKey>> CopyUserProfilePicFromDefaults(AppUserId userId) {
        var newKey = UserProfilePicKey.CreateNewForUser(userId, "webp").AsSuccess();
        var res = await CopySingleObjectAsync(source: DefaultProfilePic, destinationKey: newKey);
        if (res.IsErr(out var err)) {
            return err;
        }

        return newKey;
    }
}