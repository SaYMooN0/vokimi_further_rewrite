using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using ApiShared;
using InfrastructureShared.Storage;
using SharedKernel.auth;
using VokimiStorageService.buckets;

namespace VokimiStorageService.extensions;

internal static class BuilderServicesExtensions
{
    internal static void AddS3Storage(this IServiceCollection services, IConfiguration configuration) {
        var s3Config = configuration.GetSection("S3").Get<S3Config>();
        if (s3Config is null) {
            throw new Exception("S3 is not configured");
        }

        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
            new BasicAWSCredentials(s3Config.AccessKey, s3Config.SecretKey),
            new AmazonS3Config { ServiceURL = s3Config.ServiceUrl }
        ));

        string mainBucketName = s3Config.BucketNames["Main"] ?? throw new Exception("Main bucket is not set");
        services.AddSingleton(new MainBucketNameProvider(mainBucketName));
        services.AddScoped<MainStorageBucket>();
    }
}