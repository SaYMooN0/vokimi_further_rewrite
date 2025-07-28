using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using SharedKernel.errs;
using SharedKernel.errs.utils;
using VokimiStorageKeysLib;

namespace InfrastructureShared.Storage;

public abstract class BaseStorageBucket
{
    private readonly IAmazonS3 _s3Client;
    private readonly BaseBucketNameProvider _bucketNameProvider;
    private readonly ILogger<BaseStorageBucket> _logger;

    protected BaseStorageBucket(
        IAmazonS3 s3Client,
        BaseBucketNameProvider bucketNameProvider,
        ILogger<BaseStorageBucket> logger
    ) {
        _s3Client = s3Client;
        _bucketNameProvider = bucketNameProvider;
        _logger = logger;
    }


    protected async Task<ErrOr<TKey>> UploadWithKeyAsync<TKey>(
        Func<string, ErrOr<TKey>> keyCreationFunc, FileData file
    ) where TKey : BaseStorageKey {
        string extenstion;
        try {
            extenstion = Path.GetExtension(file.Name);
        }
        catch {
            return ErrFactory.IncorrectFormat("Unable to extract file extension");
        }

        ErrOr<TKey> keyCreation = keyCreationFunc(extenstion);
        if (keyCreation.IsErr(out var err)) {
            return err;
        }

        ErrOrNothing res = await UploadFileAsync(keyCreation.AsSuccess(), file.Stream, file.ContentType);
        if (res.IsErr(out err)) {
            return err;
        }

        return keyCreation.AsSuccess();
    }

    protected async Task<ErrOrNothing> UploadFileAsync(BaseStorageKey key, Stream content, string contentType) {
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

    protected async Task<ErrOr<(Stream Stream, string ContentType)>> GetFileAsync(string key) {
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

    protected async Task<ErrOrNothing> DeleteAsync(BaseStorageKey key) {
        var keyString = key.ToString();

        try {
            var request = new DeleteObjectRequest {
                BucketName = _bucketNameProvider.BucketName,
                Key = keyString
            };

            await _s3Client.DeleteObjectAsync(request);
            _logger.LogInformation("Successfully deleted file: {Key}", keyString);

            return ErrOrNothing.Nothing;
        }
        catch (AmazonS3Exception ex) {
            _logger.LogError(ex, "[Error] in {MethodName}, S3 exception occurred", nameof(DeleteAsync));
            return ErrFactory.Unspecified("File deletion failed");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "[Error] in {MethodName}, unexpected exception occurred", nameof(DeleteAsync));
            return ErrFactory.Unspecified("Unexpected error during file deletion");
        }
    }


    protected async Task<ErrOrNothing> CopyAsync(string source, BaseStorageKey destinationKey) {
        try {
            var request = new CopyObjectRequest {
                SourceBucket = _bucketNameProvider.BucketName,
                SourceKey = source,
                DestinationBucket = _bucketNameProvider.BucketName,
                DestinationKey = destinationKey.ToString()
            };

            await _s3Client.CopyObjectAsync(request);
            return ErrOrNothing.Nothing;
        }
        catch (AmazonS3Exception ex) {
            _logger.LogError(ex, "[Error] in {MethodName}, S3 exception occurred", nameof(CopyAsync));
            return ErrFactory.Unspecified("File copy failed");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "[Error] in {MethodName}, unexpected exception occurred", nameof(CopyAsync));
            return ErrFactory.Unspecified("Unexpected error during file copy");
        }
    }

    private static bool IsTopLevelKey(string key, string prefix) {
        var suffix = key.Substring(prefix.Length);
        return !suffix.Contains('/');
    }

    protected async Task<ErrOrNothing> DeleteFilesWithoutSubfoldersAsync(
        string prefix,
        ISet<string> usedKeys
    ) {
        List<string> deletedKeys = [];

        var request = new ListObjectsV2Request {
            BucketName = _bucketNameProvider.BucketName,
            Prefix = prefix
        };

        ListObjectsV2Response response;
        do {
            response = await _s3Client.ListObjectsV2Async(request);

            foreach (var obj in response.S3Objects) {
                if (!usedKeys.Contains(obj.Key) && IsTopLevelKey(obj.Key, prefix)) {
                    try {
                        await _s3Client.DeleteObjectAsync(new DeleteObjectRequest {
                            BucketName = _bucketNameProvider.BucketName,
                            Key = obj.Key
                        });
                        deletedKeys.Add(obj.Key);
                    }
                    catch (AmazonS3Exception ex) {
                        _logger.LogError(ex, "[Error] in {MethodName}, S3 exception occurred while deleting key {Key}",
                            nameof(DeleteFilesWithoutSubfoldersAsync), obj.Key);
                        return ErrFactory.Unspecified("File deletion failed due to S3 exception");
                    }
                    catch (Exception ex) {
                        _logger.LogError(ex,
                            "[Error] in {MethodName}, unexpected exception occurred while deleting key {Key}",
                            nameof(DeleteFilesWithoutSubfoldersAsync), obj.Key);
                        return ErrFactory.Unspecified("Unexpected error during file deletion");
                    }
                }
            }

            request.ContinuationToken = response.NextContinuationToken;
        } while (response.IsTruncated == true);

        if (deletedKeys.Count > 0) {
            _logger.LogInformation(
                "Deleted {Count} unused top-level objects under prefix '{Prefix}': {Keys}",
                deletedKeys.Count, prefix, string.Join(", ", deletedKeys)
            );
        }

        return ErrOrNothing.Nothing;
    }
}