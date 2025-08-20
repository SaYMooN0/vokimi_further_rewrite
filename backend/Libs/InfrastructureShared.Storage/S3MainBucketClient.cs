using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace InfrastructureShared.Storage;

public class S3MainBucketClient : IS3MainBucketClient
{
    private readonly IAmazonS3 _s3;
    private readonly S3MainBucketConf _s3MainBucketConf;
    private readonly ILogger<S3MainBucketClient> _logger;

    public S3MainBucketClient(IAmazonS3 s3, S3MainBucketConf s3MainBucketConf, ILogger<S3MainBucketClient> logger) {
        _s3 = s3;
        _s3MainBucketConf = s3MainBucketConf;
        _logger = logger;
    }


      public async Task<ErrOrNothing> PutFile(BaseStorageKey key, FileData file)
    {
        try
        {
            _logger.LogInformation("PUT main: bucket={Bucket} key={Key} contentType={ContentType}",
                _s3MainBucketConf.Name, key.ToString(), file.ContentType);

            PutObjectRequest req = new()
            {
                BucketName = _s3MainBucketConf.Name,
                Key = key.ToString(),
                InputStream = file.Stream,
                ContentType = file.ContentType
            };

            PutObjectResponse resp = await _s3.PutObjectAsync(req);
            _logger.LogInformation("PUT main success: key={Key} etag={ETag}", key.ToString(), resp.ETag);

            return ErrOrNothing.Nothing;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "PUT main failed: bucket={Bucket} key={Key}", _s3MainBucketConf.Name, key.ToString());
            return ErrFactory.Unspecified($"S3 Put failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PUT main unexpected error: bucket={Bucket} key={Key}", _s3MainBucketConf.Name, key.ToString());
            return ErrFactory.Unspecified($"Unexpected Put error: {ex.Message}");
        }
    }

    public async Task<ErrOrNothing> PutFile(ITempKey key, FileData file)
    {
        try
        {
            _logger.LogInformation("PUT temp: bucket={Bucket} key={Key} contentType={ContentType}",
                _s3MainBucketConf.Name, key.Value, file.ContentType);

            PutObjectRequest req = new()
            {
                BucketName = _s3MainBucketConf.Name,
                Key = key.Value,
                InputStream = file.Stream,
                ContentType = file.ContentType
            };

            PutObjectResponse resp = await _s3.PutObjectAsync(req);
            _logger.LogInformation("PUT temp success: key={Key} etag={ETag}", key.Value, resp.ETag);

            return ErrOrNothing.Nothing;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, "PUT temp failed: bucket={Bucket} key={Key}", _s3MainBucketConf.Name, key.Value);
            return ErrFactory.Unspecified($"S3 Put (temp) failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PUT temp unexpected error: bucket={Bucket} key={Key}", _s3MainBucketConf.Name, key.Value);
            return ErrFactory.Unspecified($"Unexpected Put (temp) error: {ex.Message}");
        }
    }

}