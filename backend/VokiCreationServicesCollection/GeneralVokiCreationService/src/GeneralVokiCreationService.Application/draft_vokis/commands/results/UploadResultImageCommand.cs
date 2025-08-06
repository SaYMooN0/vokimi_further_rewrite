using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.draft_general_voki.result_image;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record UploadResultImageCommand(VokiId VokiId, GeneralVokiResultId ResultId, FileData File) :
    ICommand<DraftGeneralVokiResultImageKey>,
    IWithVokiAccessValidationStep;

internal sealed class UploadResultImageCommandHandler :
    ICommandHandler<UploadResultImageCommand, DraftGeneralVokiResultImageKey>
{
    private readonly IMainStorageBucket _mainStorageBucket;

    public UploadResultImageCommandHandler(IMainStorageBucket mainStorageBucket) {
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<DraftGeneralVokiResultImageKey>> Handle(
        UploadResultImageCommand command, CancellationToken ct
    ) {
        return await _mainStorageBucket.UploadVokiResultImage(command.VokiId, command.ResultId, command.File);
    }
}