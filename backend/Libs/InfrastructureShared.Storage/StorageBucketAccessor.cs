using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;
using SharedKernel.errs;
using SharedKernel.errs.utils;
using VokimiStorageKeysLib;

namespace InfrastructureShared.Storage;

public abstract class StorageBucketAccessor
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3MainBucket _s3MainBucket;
    private readonly ILogger<StorageBucketAccessor> _logger;

    protected StorageBucketAccessor(
        IAmazonS3 s3Client,
        S3MainBucket s3MainBucket,
        ILogger<StorageBucketAccessor> logger
    ) {
        _s3Client = s3Client;
        _s3MainBucket = s3MainBucket;
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
            PutObjectRequest request = new() {
                BucketName = _s3MainBucket.Name,
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
                BucketName = _s3MainBucket.Name,
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
            DeleteObjectRequest request = new() {
                BucketName = _s3MainBucket.Name,
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


    protected async Task<ErrOrNothing> CopySingleObjectAsync(string source, BaseStorageKey destinationKey) {
        try {
            CopyObjectRequest request = new() {
                SourceBucket = _s3MainBucket.Name,
                SourceKey = source,
                DestinationBucket = _s3MainBucket.Name,
                DestinationKey = destinationKey.ToString()
            };

            await _s3Client.CopyObjectAsync(request);
            return ErrOrNothing.Nothing;
        }
        catch (AmazonS3Exception ex) {
            _logger.LogError(ex, "[Error] in {MethodName}, S3 exception occurred", nameof(CopySingleObjectAsync));
            return ErrFactory.Unspecified("File copy failed");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "[Error] in {MethodName}, unexpected exception occurred",
                nameof(CopySingleObjectAsync));
            return ErrFactory.Unspecified("Unexpected error during file copy");
        }
    }

    protected async Task<ErrOrNothing> DeleteFilesWithoutSubfoldersAsync(
        string prefix,
        ISet<string> usedKeys
    ) {
        List<string> deletedKeys = [];

        var request = new ListObjectsV2Request {
            BucketName = _s3MainBucket.Name,
            Prefix = prefix,
            Delimiter = "/"
        };

        ListObjectsV2Response response;
        do {
            response = await _s3Client.ListObjectsV2Async(request);

            foreach (var obj in response.S3Objects ?? []) {
                if (!usedKeys.Contains(obj.Key)) {
                    try {
                        await _s3Client.DeleteObjectAsync(new DeleteObjectRequest {
                            BucketName = _s3MainBucket.Name,
                            Key = obj.Key
                        });
                        deletedKeys.Add(obj.Key);
                    }
                    catch (AmazonS3Exception ex) {
                        _logger.LogError(ex,
                            "[Error] in {MethodName}, S3 exception occurred while deleting key {Key}",
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


    protected async Task<ErrOrNothing>
        CopyAllObjectsWithSubfoldersAsync(string sourcePrefix, string destinationPrefix) {
        List<string> copiedKeys = [];

        ListObjectsV2Request request = new() {
            BucketName = _s3MainBucket.Name,
            Prefix = sourcePrefix
        };

        ListObjectsV2Response response;
        do {
            response = await _s3Client.ListObjectsV2Async(request);

            foreach (var obj in response.S3Objects ?? []) {
                try {
                    var destinationKey = destinationPrefix + obj.Key.Substring(sourcePrefix.Length);

                    var copyRequest = new CopyObjectRequest {
                        SourceBucket = _s3MainBucket.Name,
                        SourceKey = obj.Key,
                        DestinationBucket = _s3MainBucket.Name,
                        DestinationKey = destinationKey
                    };

                    await _s3Client.CopyObjectAsync(copyRequest);
                    copiedKeys.Add(destinationKey);
                }
                catch (AmazonS3Exception ex) {
                    _logger.LogError(ex, "[Error] in {MethodName}, S3 exception occurred while copying key {Key}",
                        nameof(CopyAllObjectsWithSubfoldersAsync), obj.Key);
                    return ErrFactory.Unspecified("File copy failed due to S3 exception");
                }
                catch (Exception ex) {
                    _logger.LogError(ex,
                        "[Error] in {MethodName}, unexpected exception occurred while copying key {Key}",
                        nameof(CopyAllObjectsWithSubfoldersAsync), obj.Key);
                    return ErrFactory.Unspecified("Unexpected error during file copy");
                }
            }

            request.ContinuationToken = response.NextContinuationToken;
        } while (response.IsTruncated == true);

        if (copiedKeys.Count > 0) {
            _logger.LogInformation(
                "Copied {Count} objects from '{SourcePrefix}' to '{DestinationPrefix}': {Keys}",
                copiedKeys.Count, sourcePrefix, destinationPrefix, string.Join(", ", copiedKeys)
            );
        }

        return ErrOrNothing.Nothing;
    }
}