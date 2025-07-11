using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using ApiShared;
using SharedKernel.auth;
using VokimiStorageService.config;
using VokimiStorageService.storage_service.buckets;

namespace VokimiStorageService.extensions;

internal static class BuilderServicesExtensions
{
    internal static void AddUserContext(this IServiceCollection services) {
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContextProvider>();
    }

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

    public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration) {
        var jwtTokenConfig = configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfig>();
        if (jwtTokenConfig is null) {
            throw new Exception("JWT token config not configured");
        }

        services.AddSingleton(jwtTokenConfig);
        services.AddScoped<ITokenParser, TokenParser>();
        return services;
    }
}