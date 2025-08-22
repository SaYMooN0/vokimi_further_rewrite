namespace VokimiStorageService.s3_storage.images_compression;

public sealed record ImageFileAfterCompression(
    Stream Stream,
    ImageFileExtension Extension,
    string ContentType,
    bool Changed,
    long OriginalBytes,
    long ResultBytes
);