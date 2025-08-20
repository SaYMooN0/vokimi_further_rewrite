namespace InfrastructureShared.Storage;

public sealed class S3MainBucketConf
{
    public string Name { get; }

    protected S3MainBucketConf(string name) {
        Name = name;
    }
}