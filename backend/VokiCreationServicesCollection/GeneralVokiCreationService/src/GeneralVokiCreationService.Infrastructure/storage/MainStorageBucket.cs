using Amazon.S3;
using GeneralVokiCreationService.Application;
using InfrastructureShared.Storage;
using Microsoft.Extensions.Logging;
using SharedKernel.errs.utils;
using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Infrastructure.storage;

internal class MainStorageBucket : BaseStorageBucket, IMainStorageBucket
{
    public MainStorageBucket(
        IAmazonS3 s3Client,
        MainBucketNameProvider mainBucketNameProvider,
        ILogger<MainStorageBucket> logger
    ) : base(s3Client, mainBucketNameProvider, logger) { }

    public async Task<ErrOr<DraftVokiCoverKey>> UploadDraftVokiCover(
        VokiId vokiId, Stream fileStream, string fileName, string fileContentType
    ) {
        string extenstion;
        try {
            extenstion = Path.GetExtension(fileName);
        }
        catch {
            return ErrFactory.IncorrectFormat("Unable to extract file extension");
        }

        ErrOr<DraftVokiCoverKey> keyCreation = DraftVokiCoverKey.CreateWithId(vokiId, extenstion);
        if (keyCreation.IsErr(out var err)) {
            return err;
        }

        ErrOrNothing res = await base.UploadFileAsync(keyCreation.AsSuccess(), fileStream, fileContentType);
        if (res.IsErr(out err)) {
            return err;
        }

        return keyCreation;
    }

    public async Task DeleteVokiCover(DraftVokiCoverKey key) {
        if (!key.IsDefault()) {
            await base.DeleteAsync(key);
        }
    }
}