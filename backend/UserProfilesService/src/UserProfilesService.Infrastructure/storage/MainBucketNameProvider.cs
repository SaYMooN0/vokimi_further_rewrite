using InfrastructureShared.Storage;

namespace UserProfilesService.Infrastructure.storage;

internal class MainBucketNameProvider : BaseBucketNameProvider
{
    public MainBucketNameProvider(string bucketName) : base(bucketName) { }
}