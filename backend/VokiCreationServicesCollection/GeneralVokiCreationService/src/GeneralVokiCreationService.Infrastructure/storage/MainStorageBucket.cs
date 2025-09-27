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

    public Task<ErrOrNothing> CopyDefaultVokiCoverForVoki(VokiCoverKey destination) => CopyStandardToStandard(
        source: CommonStorageItemKey.DefaultVokiCover,
        destination: destination
    );

    public Task<ErrOrNothing> CopyVokiCoverFromTempToStandard(TempImageKey temp, VokiCoverKey destination) =>
        CopyTempToStandard(
            source: temp,
            destination: destination
        );

    public Task<ErrOrNothing> CopyVokiResultImageFromTempToStandard(
        TempImageKey temp, GeneralVokiResultImageKey destination
    ) => CopyTempToStandard(
        source: temp,
        destination: destination
    );

    public Task<ErrOrNothing> CopyVokiQuestionImageFromTempToStandard(
        TempImageKey temp,
        GeneralVokiQuestionImageKey destination
    ) => CopyTempToStandard(
        source: temp,
        destination: destination
    );

    public Task<ErrOrNothing> CopyVokiAnswerImageFromTempToStandard(
        TempImageKey temp,
        GeneralVokiAnswerImageKey destination
    ) => CopyTempToStandard(
        source: temp,
        destination: destination
    );

    public Task<ErrOrNothing> CopyVokiAnswerAudioFromTempToStandard(
        TempAudioKey temp,
        GeneralVokiAnswerAudioKey destination
    ) =>
        CopyTempToStandard(
            source: temp,
            destination: destination
        );
}