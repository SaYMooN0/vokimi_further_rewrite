using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.draft_general_voki.question_image;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record UploadVokiQuestionImageCommand(VokiId VokiId, GeneralVokiQuestionId QuestionId, FileData File) :
    ICommand<DraftGeneralVokiQuestionImageKey>,
    IWithVokiAccessValidationStep;

internal sealed class UploadVokiQuestionImageCommandHandler :
    ICommandHandler<UploadVokiQuestionImageCommand, DraftGeneralVokiQuestionImageKey>
{
    private readonly IMainStorageBucket _mainStorageBucket;

    public UploadVokiQuestionImageCommandHandler(IMainStorageBucket mainStorageBucket) {
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<DraftGeneralVokiQuestionImageKey>> Handle(
        UploadVokiQuestionImageCommand command, CancellationToken ct
    ) =>
        await _mainStorageBucket.UploadVokiQuestionImage(command.VokiId, command.QuestionId, command.File);
}