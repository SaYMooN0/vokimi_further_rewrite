using VokimiStorageService.s3_storage.images_compression;
using VokimiStorageService.s3_storage.s3;

namespace VokimiStorageService.s3_storage.storage_service;

internal sealed class StorageService : IStorageService
{
    private readonly IS3MainBucketClient _s3;
    private readonly IImageFileCompressor _compressor;
    private readonly ILogger<StorageService> _logger;

    private const long MB = 1024L * 1024L;
    private const long MaxUploadBytes = 25L * MB;

    public StorageService(
        IS3MainBucketClient s3,
        IImageFileCompressor compressor,
        ILogger<StorageService> logger
    ) {
        _s3 = s3;
        _compressor = compressor;
        _logger = logger;
    }

    public async Task<ErrOr<TempImageKey>> PutTempImageFile(
        FileData data, CancellationToken ct = default
    ) {
        try {
            var seekableData = await EnsureSeekableAsync(data, ct);
            var sizeBytes = seekableData.Stream.Length;

            if (sizeBytes > MaxUploadBytes) {
                _logger.LogWarning("PutTempImageFile: file too large ({Size} bytes)", sizeBytes);
                return ErrFactory.Conflict($"Image exceeds {MaxUploadBytes / MB} MB limit");
            }

            var compressedOrErr = await _compressor.CompressAsync(seekableData, ct);
            if (compressedOrErr.IsErr(out var err)) {
                _logger.LogError("PutTempImageFile: compression failed: {Error}", err.Message);
                return err;
            }

            ImageFileAfterCompression compressed = compressedOrErr.AsSuccess();

            _logger.LogInformation(
                "PutTempImageFile: compression done. Changed={Changed}, {Original}B -> {Result}B, Ext={Ext}, ContentType={CT}",
                compressed.Changed, compressed.OriginalBytes, compressed.ResultBytes,
                compressed.Extension, compressed.ContentType
            );

            var tempKey = TempImageKey.CreateWithExtension(compressed.Extension);

            if (compressed.Stream.CanSeek) {
                compressed.Stream.Position = 0;
            }

            var putErr = await _s3.PutFile(
                tempKey,
                new FileData(compressed.Stream, compressed.ContentType),
                ct
            );

            if (putErr.IsErr(out err)) {
                _logger.LogError("PutTempImageFile: S3 put failed for {TempKey}: {Error}",
                    tempKey, err.Message);
                return err;
            }

            _logger.LogInformation("PutTempImageFile: uploaded to temp {TempKey}, bytes={Bytes}",
                tempKey, compressed.ResultBytes);

            return tempKey;
        }
        catch (OperationCanceledException) {
            _logger.LogWarning("PutTempImageFile: cancelled");
            throw;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "PutTempImageFile: unexpected error");
            return ErrFactory.Conflict("Unexpected error while uploading image");
        }
    }

    private static async Task<FileData> EnsureSeekableAsync(FileData data, CancellationToken ct) {
        if (data.Stream.CanSeek) {
            return data;
        }

        MemoryStream ms = new();
        await data.Stream.CopyToAsync(ms, ct);
        ms.Position = 0;
        return data with { Stream = ms };
    }
}