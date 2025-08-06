using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.draft_general_voki.result_image;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record UploadVokiResultImageCommand(VokiId VokiId, GeneralVokiResultId ResultId, FileData File) :
    ICommand<DraftGeneralVokiResultImageKey>,
    IWithVokiAccessValidationStep;

internal sealed class UploadVokiResultImageCommandHandler :
    ICommandHandler<UploadVokiResultImageCommand, DraftGeneralVokiResultImageKey>
{
    private readonly IMainStorageBucket _mainStorageBucket;

    public UploadVokiResultImageCommandHandler(IMainStorageBucket mainStorageBucket) {
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<DraftGeneralVokiResultImageKey>> Handle(
        UploadVokiResultImageCommand command, CancellationToken ct
    ) =>
        await _mainStorageBucket.UploadVokiResultImage(command.VokiId, command.ResultId, command.File);
}