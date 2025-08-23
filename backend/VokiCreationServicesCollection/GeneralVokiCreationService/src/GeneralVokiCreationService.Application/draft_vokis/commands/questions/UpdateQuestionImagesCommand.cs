using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.extension;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record UpdateQuestionImagesCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    TempImageKey[] NewImages
) :
    ICommand<VokiQuestionImagesSet>,
    IWithVokiAccessValidationStep;

internal sealed class
    UpdateQuestionImagesCommandHandler : ICommandHandler<UpdateQuestionImagesCommand, VokiQuestionImagesSet>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public UpdateQuestionImagesCommandHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository, IMainStorageBucket mainStorageBucket
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<VokiQuestionImagesSet>> Handle(UpdateQuestionImagesCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestions(command.VokiId))!;

        if (VokiQuestionImagesSet.CheckForErr(command.NewImages.Length).IsErr(out var err)) {
            return err;
        }

        List<GeneralVokiQuestionImageKey> resultList = new(command.NewImages.Length);
        foreach (var tempKey in command.NewImages) {
            ImageFileExtension ext = tempKey.Extension;
            var destination = GeneralVokiQuestionImageKey.CreateForQuestion(command.VokiId, command.QuestionId, ext);
            var copyingRes = await _mainStorageBucket.CopyVokiQuestionImageFromTempToStandard(
                tempKey, destination
            );
            if (copyingRes.IsErr(out err)) {
                return ErrFactory.Unspecified("Couldn't update question images from", details: err.Message);
            }

            resultList.Add(destination);
        }

        var imagesSetRes = VokiQuestionImagesSet.Create(resultList.ToImmutableArray());
        if (imagesSetRes.IsErr(out err)) {
            return err;
        }

        var res = voki.UpdateQuestionImages(command.QuestionId, imagesSetRes.AsSuccess());
        if (res.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return res.AsSuccess().Images;
    }
}