namespace InfrastructureShared.Storage;

public class S3MainBucket
{
    public string Name { get; }

    protected S3MainBucket(string name) {
        Name = name;
    }
}