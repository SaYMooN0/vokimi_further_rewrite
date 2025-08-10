using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.general_voki.question_image;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record UploadVokiQuestionImageCommand(VokiId VokiId, GeneralVokiQuestionId QuestionId, FileData File) :
    ICommand<GeneralVokiQuestionImageKey>,
    IWithVokiAccessValidationStep;

internal sealed class UploadVokiQuestionImageCommandHandler :
    ICommandHandler<UploadVokiQuestionImageCommand, GeneralVokiQuestionImageKey>
{
    private readonly IMainStorageBucket _mainStorageBucket;

    public UploadVokiQuestionImageCommandHandler(IMainStorageBucket mainStorageBucket) {
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<GeneralVokiQuestionImageKey>> Handle(
        UploadVokiQuestionImageCommand command, CancellationToken ct
    ) =>
        await _mainStorageBucket.UploadVokiQuestionImage(command.VokiId, command.QuestionId, command.File);
}