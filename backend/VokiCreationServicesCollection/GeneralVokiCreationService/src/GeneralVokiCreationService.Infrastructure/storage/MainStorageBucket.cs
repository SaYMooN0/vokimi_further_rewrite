using Amazon.S3;
using GeneralVokiCreationService.Application;
using GeneralVokiCreationService.Application.common;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Infrastructure.storage;

internal class MainStorageBucket : BaseMainS3Bucket, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        S3MainBucketConf s3MainBucketConf,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, s3MainBucketConf, logger) { }

    public Task<ErrOrNothing> CopyDefaultVokiCoverForVoki(
        VokiCoverKey destination, CancellationToken ct
    ) => CopyStandardToStandard(
        source: CommonStorageItemKey.DefaultVokiCover,
        destination: destination,
        ct: ct
    );

    public Task<ErrOrNothing> CopyVokiCoverFromTempToStandard(
        TempImageKey temp,
        VokiCoverKey destination,
        CancellationToken ct
    ) =>
        CopyTempToStandard(
            source: temp,
            destination: destination,
            ct: ct
        );

    public Task<ErrOrNothing> CopyVokiQuestionImagesFromTempToStandard(
        Dictionary<TempImageKey, GeneralVokiQuestionImageKey> tempToDestination, CancellationToken ct
    ) =>
        CopyMultipleTempToStandard(sourcesToDestinations: tempToDestination, ct);

    public Task<ErrOrNothing> CopyVokiResultImageFromTempToStandard(
        TempImageKey temp,
        GeneralVokiResultImageKey destination,
        CancellationToken ct
    ) =>
        CopyTempToStandard(
            source: temp,
            destination: destination,
            ct
        );


    public Task<ErrOrNothing> CopyVokiAnswerImageFromTempToStandard(
        TempImageKey temp,
        GeneralVokiAnswerImageKey destination,
        CancellationToken ct
    ) =>
        CopyTempToStandard(
            source: temp,
            destination: destination,
            ct
        );

    public Task<ErrOrNothing> CopyVokiAnswerAudioFromTempToStandard(
        TempAudioKey temp,
        GeneralVokiAnswerAudioKey destination,
        CancellationToken ct
    ) =>
        CopyTempToStandard(
            source: temp,
            destination: destination,
            ct
        );
}