namespace InfrastructureShared.Storage;

public class S3Config
{
    public string ServiceUrl { get; init; } = null!;
    public string AccessKey { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public Dictionary<string, string> BucketNames { get; init; } = null!;
}