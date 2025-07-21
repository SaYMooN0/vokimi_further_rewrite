using Amazon.S3;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using UserProfilesService.Application;
using UserProfilesService.Application.app_users;
using VokimiStorageKeysLib.users;

namespace UserProfilesService.Infrastructure.storage;

internal class MainStorageBucket : BaseStorageBucket, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        MainBucketNameProvider mainBucketNameProvider,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, mainBucketNameProvider, logger) { }

    public Task<ErrOr<UserProfilePicKey>> CopyUserProfilePicFromDefaults(AppUserId userId) => throw new NotImplementedException();

    public Task DeleteUserProfilePic(UserProfilePicKey key) => throw new NotImplementedException();
}