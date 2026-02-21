using Xabe.FFmpeg;

namespace VokimiStorageService.s3_storage.audio_compression;

internal sealed class AudioFileConverter : IAudioFileConverter
{
    private readonly ILogger<AudioFileConverter> _logger;

    private const long Mp3SizeThresholdBytes = 5L * 1024L * 1024L; // 5 MB

    public AudioFileConverter(ILogger<AudioFileConverter> logger, IConfiguration configuration) {
        FFmpeg.SetExecutablesPath(configuration["FfmpegPath"]);
        _logger = logger;
    }

    public async Task<ErrOr<AudioFileAfterConversion>> ConvertAudioAsync(FileData data, CancellationToken ct) {
        try {
            long originalSizeBytes = data.Stream.Length;
            bool isMp3 = data.ContentType.Contains("mpeg") || data.ContentType.Contains("mp3");

            if (isMp3 && originalSizeBytes < Mp3SizeThresholdBytes) {
                _logger.LogInformation("ConvertAudioAsync: MP3 is below threshold ({Bytes} B). Returning as is.",
                    originalSizeBytes);
                return new AudioFileAfterConversion(
                    Stream: data.Stream,
                    ContentType: "audio/mpeg",
                    Extension: "mp3",
                    Changed: false,
                    OriginalBytes: originalSizeBytes,
                    ResultBytes: originalSizeBytes
                );
            }

            string inputTempPath = Path.GetTempFileName();
            string outputExt = isMp3 ? ".mp3" : ".m4a";
            string outputTempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + outputExt);

            try {
                if (data.Stream.CanSeek) {
                    data.Stream.Position = 0;
                }

                await using (var fileStream = new FileStream(inputTempPath, FileMode.Create, FileAccess.Write)) {
                    await data.Stream.CopyToAsync(fileStream, ct);
                }

                _logger.LogInformation("ConvertAudioAsync: Starting FFmpeg conversion to {Ext} (Original Size: {Bytes} B)",
                    outputExt, originalSizeBytes);

                var mediaInfo = await FFmpeg.GetMediaInfo(inputTempPath, ct);
                var audioStream = mediaInfo.AudioStreams.FirstOrDefault();

                if (audioStream == null) {
                    return ErrFactory.Conflict("No audio stream found in the uploaded file.");
                }

                var conversion = FFmpeg.Conversions.New()
                    .AddStream(audioStream)
                    .SetOutput(outputTempPath);

                if (isMp3) {
                    conversion.SetAudioBitrate(128000); // 128 kbps
                }
                else {
                    audioStream.SetCodec(AudioCodec.aac);
                }

                await conversion.Start(ct);

                var convertedStream = new FileStream(
                    outputTempPath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
                    FileOptions.DeleteOnClose
                );
                long resultBytes = convertedStream.Length;

                _logger.LogInformation("ConvertAudioAsync: FFmpeg conversion successful. Result Size: {Bytes} B", resultBytes);

                return new AudioFileAfterConversion(
                    Stream: convertedStream,
                    ContentType: isMp3 ? "audio/mpeg" : "audio/mp4",
                    Extension: isMp3 ? "mp3" : "m4a",
                    Changed: true,
                    OriginalBytes: originalSizeBytes,
                    ResultBytes: resultBytes
                );
            }
            finally {
                if (File.Exists(inputTempPath)) {
                    File.Delete(inputTempPath);
                }
                // Don't delete outputTempPath here as it's bound to the FileStream with DeleteOnClose.
            }
        }
        catch (Exception ex) {
            _logger.LogError(ex, "ConvertAudioAsync: Error converting audio.");
            return ErrFactory.Conflict("Failed to convert audio file format.");
        }
    }
}