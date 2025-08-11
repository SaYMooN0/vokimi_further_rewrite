using InfrastructureShared.Storage;

namespace GeneralVokiTakingService.Infrastructure.storage;

internal class MainBucketNameProvider : BaseBucketNameProvider
{
    public MainBucketNameProvider(string bucketName) : base(bucketName) { }
}