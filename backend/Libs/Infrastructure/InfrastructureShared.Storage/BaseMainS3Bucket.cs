using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Logging;

namespace InfrastructureShared.Storage;

public abstract class BaseMainS3Bucket
{
    private readonly IAmazonS3 _s3;
    private readonly S3MainBucketConf _s3MainBucketConf;
    private readonly ILogger<BaseMainS3Bucket> _logger;

    protected BaseMainS3Bucket(IAmazonS3 s3, S3MainBucketConf s3MainBucketConf, ILogger<BaseMainS3Bucket> logger) {
        _s3 = s3;
        _s3MainBucketConf = s3MainBucketConf;
        _logger = logger;
    }

    protected Task<ErrOrNothing> CopyTempToStandard(
        ITempKey source,
        BaseStorageKey destination,
        CancellationToken ct = default
    ) => BaseCopy(
        new FileCopyDto(source.Value, source.Extension.Value),
        new FileCopyDto(destination.ToString(), destination.Extension.Value),
        logsString: "COPY temp->standard",
        notFoundUserMessage: "Temp source object not found for copy",
        ct: ct
    );

    public Task<ErrOrNothing> CopyStandardToStandard(
        BaseStorageKey source,
        BaseStorageKey destination,
        CancellationToken ct = default
    ) => BaseCopy(
        new FileCopyDto(source.ToString(), source.Extension.Value),
        new FileCopyDto(destination.ToString(), destination.Extension.Value),
        logsString: "COPY standard->standard",
        notFoundUserMessage: "Source object not found for copy",
        ct: ct
    );

    private record class FileCopyDto(string Key, string Ext);

    private async Task<ErrOrNothing> BaseCopy(
        FileCopyDto src,
        FileCopyDto dest,
        string logsString,
        string notFoundUserMessage,
        CancellationToken ct = default
    ) {
        if (src.Ext != dest.Ext) {
            _logger.LogError(
                "{Log} conflict: ext mismatch srcExt={SrcExt} dstExt={DstExt} srcKey={Src} dstKey={Dst}",
                logsString, src.Ext, dest.Ext, src.Key, dest.Key
            );
            return ErrFactory.Conflict($"Extension mismatch: '{src.Ext}' -> '{dest.Ext}'");
        }

        try {
            var req = new CopyObjectRequest {
                SourceBucket = _s3MainBucketConf.Name,
                SourceKey = src.Key,
                DestinationBucket = _s3MainBucketConf.Name,
                DestinationKey = dest.Key,
                MetadataDirective = S3MetadataDirective.COPY
            };

            var resp = await _s3.CopyObjectAsync(req, ct);
            _logger.LogInformation("{Log} success: etag={ETag} dstKey={Dst}", logsString, resp.ETag, dest.Key);

            return ErrOrNothing.Nothing;
        }
        catch (AmazonS3Exception ex) {
            if (ex.StatusCode == HttpStatusCode.NotFound) {
                _logger.LogError("{Log} failed NotFound: srcKey={Src}", logsString, src.Key);
                return ErrFactory.NotFound.Common(
                    notFoundUserMessage,
                    details: $"No file with key: {src.Key}"
                );
            }

            _logger.LogError(ex, "{Log} failed: src={Src} dst={Dst}", logsString, src.Key, dest.Key);
            return ErrFactory.Unspecified("File copy failed");
        }
        catch (Exception ex) {
            _logger.LogError(ex, "{Log} unexpected error: src={Src} dst={Dst}", logsString, src.Key, dest.Key);
            return ErrFactory.Unspecified("File copy failed");
        }
    }
}