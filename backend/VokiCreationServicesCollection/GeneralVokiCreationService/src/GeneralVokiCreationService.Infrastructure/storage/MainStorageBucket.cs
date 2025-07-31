using System.Collections.Immutable;
using Amazon.S3;
using GeneralVokiCreationService.Application;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.draft_general_voki.question_image;
using VokimiStorageKeysLib.draft_general_voki.result_image;
using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Infrastructure.storage;

internal class MainStorageBucket : BaseStorageBucket, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        MainBucketNameProvider mainBucketNameProvider,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, mainBucketNameProvider, logger) { }

    public async Task<ErrOr<DraftVokiCoverKey>> UploadDraftVokiCover(VokiId vokiId, FileData file) =>
        await UploadWithKeyAsync(
            (ext) => DraftVokiCoverKey.CreateWithId(vokiId, ext),
            file
        );

    public async Task<ErrOrNothing> DeleteVokiCover(DraftVokiCoverKey key) {
        if (!key.IsDefault()) {
            return await base.DeleteAsync(key);
        }

        return ErrOrNothing.Nothing;
    }

    public async Task<ErrOr<DraftGeneralVokiQuestionImageKey>> UploadVokiQuestionImage(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        FileData file
    ) =>
        await UploadWithKeyAsync<DraftGeneralVokiQuestionImageKey>(
            (ext) => DraftGeneralVokiQuestionImageKey.Create(vokiId, questionId, ext),
            file
        );

    public async Task<ErrOrNothing> DeleteUnusedQuestionImages(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        IEnumerable<DraftGeneralVokiQuestionImageKey> usedKeys
    ) {
        string prefix = DraftGeneralVokiQuestionImageKey.Folder(vokiId, questionId) + '/';
        var usedStringifiedKeys = usedKeys
            .Select(k => k.ToString())
            .ToImmutableHashSet();
        return await base.DeleteFilesWithoutSubfoldersAsync(prefix, usedStringifiedKeys);
    }

    public async Task<ErrOr<DraftGeneralVokiResultImageKey>> UploadVokiResultImage(
        VokiId vokiId,
        GeneralVokiResultId resultId,
        FileData file
    ) => await UploadWithKeyAsync<DraftGeneralVokiResultImageKey>(
        (ext) => DraftGeneralVokiResultImageKey.Create(vokiId, resultId, ext),
        file
    );
}