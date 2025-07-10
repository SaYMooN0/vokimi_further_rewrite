using Amazon.Runtime;
using Amazon.S3;
using ApiShared;
using SharedKernel.auth;

namespace VokimiStorageService.extensions;

internal static class BuilderServicesExtensions
{
    internal static void AddUserContext(this IServiceCollection services) {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContextProvider>();
    }

    internal static void AddS3Storage(this IServiceCollection services, IConfiguration configuration) {
        string serviceUrl = configuration["S3:ServiceURL"] ?? throw new Exception("S3:ServiceURL is not set");
        string accessKey = configuration["S3:AccessKey"] ?? throw new Exception("S3:AccessKey is not set");
        string secretKey = configuration["S3:SecretKey"] ?? throw new Exception("S3:SecretKey is not set");
        string bucketName = configuration["S3:BucketName"] ?? throw new Exception("S3:BucketName is not set");



        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
            new BasicAWSCredentials(accessKey, secretKey),
            new AmazonS3Config { ServiceURL = serviceUrl }
        ));
        // services.AddScoped(sp => new VokimiStorageService(sp.GetRequiredService<IAmazonS3>(), bucketName));
    }
}