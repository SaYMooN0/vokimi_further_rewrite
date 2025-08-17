using System.Collections.Immutable;
using Amazon.S3;
using GeneralVokiCreationService.Application;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.general_voki.question_image;
using VokimiStorageKeysLib.general_voki.result_image;
using VokimiStorageKeysLib.voki_cover;

namespace GeneralVokiCreationService.Infrastructure.storage;

internal class MainStorageBucket : BaseStorageBucket, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        MainBucketNameProvider mainBucketNameProvider,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, mainBucketNameProvider, logger) { }

    public async Task<ErrOr<VokiCoverKey>> UploadVokiCover(VokiId vokiId, FileData file) =>
        await UploadWithKeyAsync(
            (ext) => VokiCoverKey.CreateWithId(vokiId, ext),
            file
        );

    public async Task<ErrOrNothing> DeleteVokiCover(VokiCoverKey key) {
        if (!key.IsDefault()) {
            return await base.DeleteAsync(key);
        }

        return ErrOrNothing.Nothing;
    }

    public async Task<ErrOr<GeneralVokiQuestionImageKey>> UploadVokiQuestionImage(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        FileData file
    ) =>
        await UploadWithKeyAsync<GeneralVokiQuestionImageKey>(
            (ext) => GeneralVokiQuestionImageKey.Create(vokiId, questionId, ext),
            file
        );

    public async Task<ErrOrNothing> DeleteUnusedQuestionImages(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        IEnumerable<GeneralVokiQuestionImageKey> usedKeys
    ) {
        string prefix = GeneralVokiQuestionImageKey.Folder(vokiId, questionId) + '/';
        var usedStringifiedKeys = usedKeys
            .Select(k => k.ToString())
            .ToImmutableHashSet();
        return await base.DeleteFilesWithoutSubfoldersAsync(prefix, usedStringifiedKeys);
    }

    public async Task<ErrOrNothing> DeleteUnusedResultImages(
        VokiId vokiId,
        GeneralVokiResultId resultId,
        GeneralVokiResultImageKey? currentKey
    ) {
        string prefix = GeneralVokiResultImageKey.Folder(vokiId, resultId) + '/';
        ImmutableHashSet<string> usedStringifiedKeys = currentKey is null ? [] : [currentKey.ToString()];
        return await base.DeleteFilesWithoutSubfoldersAsync(prefix, usedStringifiedKeys);
    }

    public async Task<ErrOr<GeneralVokiResultImageKey>> UploadVokiResultImage(
        VokiId vokiId,
        GeneralVokiResultId resultId,
        FileData file
    ) => await UploadWithKeyAsync<GeneralVokiResultImageKey>(
        (ext) => GeneralVokiResultImageKey.Create(vokiId, resultId, ext),
        file
    );

}