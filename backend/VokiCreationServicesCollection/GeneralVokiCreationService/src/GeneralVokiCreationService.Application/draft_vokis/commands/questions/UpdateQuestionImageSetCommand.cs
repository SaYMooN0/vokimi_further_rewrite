using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.extension;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record UpdateQuestionImageSetCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    TempImageKey[] TempKeys,
    GeneralVokiQuestionImageKey[] SavedKeys,
    VokiQuestionImagesAspectRatio ImagesAspectRatio
) :
    ICommand<VokiQuestionImagesSet>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateQuestionImageSetCommandHandler :
    ICommandHandler<UpdateQuestionImageSetCommand, VokiQuestionImagesSet>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public UpdateQuestionImageSetCommandHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository, IMainStorageBucket mainStorageBucket
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<VokiQuestionImagesSet>> Handle(UpdateQuestionImageSetCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestions(command.VokiId))!;

        if (Validate(command.QuestionId, command.TempKeys, command.SavedKeys).IsErr(out var err)) {
            return err;
        }

        List<GeneralVokiQuestionImageKey> resultKeys = [..command.SavedKeys];

        foreach (TempImageKey tempKey in command.TempKeys) {
            ImageFileExtension ext = tempKey.Extension;
            var destination = GeneralVokiQuestionImageKey.CreateForQuestion(command.VokiId, command.QuestionId, ext);
            var copyingRes = await _mainStorageBucket.CopyVokiQuestionImageFromTempToStandard(
                tempKey, destination
            );
            if (copyingRes.IsErr(out err)) {
                return ErrFactory.Unspecified("Couldn't update question images from", details: err.Message);
            }

            resultKeys.Add(destination);
        }

        ErrOr<VokiQuestionImagesSet> imagesSetRes = VokiQuestionImagesSet.Create(
            [..resultKeys], command.ImagesAspectRatio
        );
        if (imagesSetRes.IsErr(out err)) {
            return err;
        }

        var res = voki.UpdateQuestionImages(command.QuestionId, imagesSetRes.AsSuccess());
        if (res.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return res.AsSuccess().ImageSet;
    }

    private static ErrOrNothing Validate(
        GeneralVokiQuestionId questionId,
        TempImageKey[] tempKeys, GeneralVokiQuestionImageKey[] savedKeys
    ) {
        int totalCount = tempKeys.Length + savedKeys.Length;
        if (VokiQuestionImagesSet.CheckForErr(totalCount).IsErr(out var err)) {
            return err;
        }

        if (savedKeys.Any(k => k.QuestionId != questionId)) {
            return ErrFactory.Conflict("Not all saved images belong to this question");
        }

        return ErrOrNothing.Nothing;
    }
}