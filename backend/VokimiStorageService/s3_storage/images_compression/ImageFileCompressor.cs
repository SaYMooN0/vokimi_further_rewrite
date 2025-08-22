using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Webp;

namespace VokimiStorageService.s3_storage.images_compression;

internal sealed class ImageFileCompressor : IImageFileCompressor
{
    public async Task<ErrOr<ImageFileAfterCompression>> CompressAsync(
        FileData file, CancellationToken ct = default
    ) {
        try {
            var extOrErr = GetExtensionFromContentType(file.ContentType);
            if (extOrErr.IsErr(out var err)) {
                return err;
            }

            string ext = extOrErr.AsSuccess();
            var originalBytes = file.Stream.Length;

            var decisionOrErr = ImageCompressionPolicy.Decide(originalBytes, ext);
            if (decisionOrErr.IsErr(out err)) {
                return err;
            }

            ImageCompressionDecision decision = decisionOrErr.AsSuccess();
            file.Stream.Position = 0;

            // if passthrough return as is
            if (!decision.ShouldTranscode || decision.TargetFormat == TargetImageFormat.Passthrough) {
                return new ImageFileAfterCompression(
                    Stream: file.Stream,
                    Extension: ImageFileExtension.Create(ext).AsSuccess(),
                    ContentType: file.ContentType,
                    Changed: false,
                    OriginalBytes: originalBytes,
                    ResultBytes: originalBytes
                );
            }

            using var image = await Image.LoadAsync(file.Stream, ct);
            var ms = new MemoryStream();

            IImageEncoder encoder = decision.TargetFormat switch {
                TargetImageFormat.Jpeg => new JpegEncoder {
                    Quality = decision.Quality ?? 85
                },
                TargetImageFormat.WebpLossless => new WebpEncoder {
                    FileFormat = WebpFileFormatType.Lossless
                },
                TargetImageFormat.WebpLossy => new WebpEncoder {
                    FileFormat = WebpFileFormatType.Lossy,
                    Quality = decision.Quality ?? 80
                },
                _ => throw new InvalidOperationException($"Unsupported target {decision.TargetFormat}")
            };

            await image.SaveAsync(ms, encoder, ct);
            ms.Position = 0;

            string contentType = decision.TargetFormat switch {
                TargetImageFormat.Jpeg => "image/jpeg",
                TargetImageFormat.WebpLossless or TargetImageFormat.WebpLossy => "image/webp",
                _ => file.ContentType
            };

            return new ImageFileAfterCompression(
                Stream: ms,
                Extension: ToExtensionFromTarget(decision.TargetFormat),
                ContentType: contentType,
                Changed: true,
                OriginalBytes: originalBytes,
                ResultBytes: ms.Length
            );
        }
        catch (Exception ex) {
            return ErrFactory.Conflict($"Compression failed: {ex.Message}");
        }
    }

    private static ImageFileExtension ToExtensionFromTarget(TargetImageFormat target) =>
        target switch {
            TargetImageFormat.Jpeg => ImageFileExtension.Jpeg,
            TargetImageFormat.WebpLossless or TargetImageFormat.WebpLossy => ImageFileExtension.Webp,
            _ => throw new ArgumentException($"Unsupported target {target}")
        };

    private static ErrOr<string> GetExtensionFromContentType(string contentType) =>
        contentType.ToLowerInvariant() switch {
            "image/png" => "png",
            "image/jpeg" or "image/jpg" => "jpeg",
            "image/webp" => "webp",
            _ => ErrFactory.Conflict($"Unsupported content type '{contentType}'")
        };
}