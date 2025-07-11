using Amazon.S3;
using Amazon.S3.Model;
using SharedKernel.errs;
using SharedKernel.errs.utils;
using SharedKernel.exceptions;
using VokimiStorageKeysLib;

namespace VokimiStorageService.storage_service;

public abstract class BaseStorageBucket
{
    private readonly IAmazonS3 _s3Client;
    private readonly BaseBucketNameProvider _bucketNameProvider;
    private readonly ILogger<BaseStorageBucket> _logger;

    public BaseStorageBucket(
        IAmazonS3 s3Client,
        BaseBucketNameProvider bucketNameProvider,
        ILogger<BaseStorageBucket> logger
    ) {
        _s3Client = s3Client;
        _bucketNameProvider = bucketNameProvider;
        _logger = logger;
    }

    public async Task<ErrOrNothing> UploadFileAsync(BaseStorageKey key, Stream content, string contentType) {
        try {
            var request = new PutObjectRequest {
                BucketName = _bucketNameProvider.BucketName,
                Key = key.ToString(),
                InputStream = content,
                ContentType = contentType
            };

            await _s3Client.PutObjectAsync(request);
            return ErrOrNothing.Nothing;
        }
        catch (AmazonS3Exception ex) {
            _logger.LogError(ex, "[Error] in {MethodName}, S3 exception occurred", nameof(UploadFileAsync));
            return ErrFactory.Unspecified("File upload failed");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "[Error] in {MethodName}, unexpected exception occurred", nameof(UploadFileAsync));
            return ErrFactory.Unspecified("Unexpected error during file upload");
        }
    }

    public async Task<ErrOr<(Stream Stream, string ContentType)>> GetFileAsync(string key) {
        try {
            GetObjectRequest request = new() {
                BucketName = _bucketNameProvider.BucketName,
                Key = key
            };
            var response = await _s3Client.GetObjectAsync(request);
            return (response.ResponseStream, response.Headers.ContentType);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound) {
            return ErrFactory.NotFound.Common("Requested file was not found");
        }
        catch (AmazonS3Exception ex) {
            _logger.LogError(ex, "[Error] in {MethodName}, S3 exception occurred", nameof(GetFileAsync));
            return ErrFactory.NotFound.Common("File download failed");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "[Error] in {MethodName}, unexpected exception occurred", nameof(GetFileAsync));
            return ErrFactory.Unspecified("Unexpected error during file download");
        }
    }
}