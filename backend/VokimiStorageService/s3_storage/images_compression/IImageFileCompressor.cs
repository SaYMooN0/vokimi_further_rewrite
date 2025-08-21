namespace InfrastructureShared.Storage.images_compression;

internal interface IImageFileCompressor
{
    Task<ErrOr<ImageFileAfterCompression>> CompressAsync(FileData file, CancellationToken ct = default);
}