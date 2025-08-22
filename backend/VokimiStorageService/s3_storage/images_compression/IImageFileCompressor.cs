namespace VokimiStorageService.s3_storage.images_compression;

internal interface IImageFileCompressor
{
    Task<ErrOr<ImageFileAfterCompression>> CompressAsync(FileData file, CancellationToken ct = default);
}