using InfrastructureShared.Storage;

namespace GeneralVokiCreationService.Infrastructure.storage;

public class MainBucketNameProvider : BaseBucketNameProvider
{
    public MainBucketNameProvider(string bucketName) : base(bucketName) { }
}