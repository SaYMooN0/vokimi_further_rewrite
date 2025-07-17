using InfrastructureShared.Storage;

namespace GeneralVokiCreationService.Infrastructure.storage;

internal class MainBucketNameProvider : BaseBucketNameProvider
{
    public MainBucketNameProvider(string bucketName) : base(bucketName) { }
}