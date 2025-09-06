using GeneralVokiCreationService.Application.dtos;
using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using SharedKernel.common.vokis;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.extension;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.answers;

public sealed record AddNewAnswerToVokiQuestionCommand(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    VokiAnswerTypeDataDto AnswerDataDto,
    ImmutableHashSet<GeneralVokiResultId> RelatedResultIds
) :
    ICommand<VokiQuestionAnswer>,
    IWithVokiAccessValidationStep;

internal sealed class AddNewAnswerToVokiQuestionCommandHandler :
    ICommandHandler<AddNewAnswerToVokiQuestionCommand, VokiQuestionAnswer>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public AddNewAnswerToVokiQuestionCommandHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        IMainStorageBucket mainStorageBucket
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<VokiQuestionAnswer>> Handle(
        AddNewAnswerToVokiQuestionCommand command, CancellationToken ct
    ) {
        ErrOr<BaseVokiAnswerTypeData> answerDataRes =
            await HandleAnswerData(command.VokiId, command.QuestionId, command.AnswerDataDto);
        if (answerDataRes.IsErr(out var err)) {
            return err;
        }

        BaseVokiAnswerTypeData answerData = answerDataRes.AsSuccess();
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestionAnswersAndResults(command.VokiId))!;

        var res = voki.AddNewAnswerToQuestion(
            command.QuestionId, answerData, command.RelatedResultIds
        );
        if (res.IsErr(out err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki);
        return res.AsSuccess();
    }

    private Task<ErrOr<BaseVokiAnswerTypeData>> HandleAnswerData(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        VokiAnswerTypeDataDto d
    ) => d.Type.Match(
        textOnly: () => Task.FromResult(
            d.GetText().Bind<BaseVokiAnswerTypeData>(text => new BaseVokiAnswerTypeData.TextOnly(text))
        ),
        imageOnly: () => CreateImageOnlyAnswerData(vokiId, questionId, d),
        imageAndText: () => CreateImageAndTextAnswerData(vokiId, questionId, d),
        colorOnly: () => Task.FromResult(
            d.GetColor().Bind<BaseVokiAnswerTypeData>(color => new BaseVokiAnswerTypeData.ColorOnly(color))),
        colorAndText: () => Task.FromResult(
            d.GetText().Bind<BaseVokiAnswerTypeData>(text =>
                d.GetColor().Bind<BaseVokiAnswerTypeData>(color => new BaseVokiAnswerTypeData.ColorAndText(text, color))
            )),
        audioOnly: () => CreateAudioOnlyAnswerData(vokiId, questionId, d),
        audioAndText: () => CreateAudioAndTextAnswerData(vokiId, questionId, d)
    );


    private async Task<ErrOr<BaseVokiAnswerTypeData>> CreateImageOnlyAnswerData(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        VokiAnswerTypeDataDto answer
    ) => (
        await HandleAnswerImageKey(vokiId, questionId, answer.Image)
    ).Bind<BaseVokiAnswerTypeData>(savedKey => new BaseVokiAnswerTypeData.ImageOnly(savedKey));

    private async Task<ErrOr<BaseVokiAnswerTypeData>> CreateImageAndTextAnswerData(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        VokiAnswerTypeDataDto answer
    ) {
        ErrOr<GeneralVokiAnswerText> textRes = answer.GetText();
        if (textRes.IsErr(out var err)) {
            return err;
        }

        ErrOr<GeneralVokiAnswerImageKey> keyRes = await HandleAnswerImageKey(vokiId, questionId, answer.Image);
        if (keyRes.IsErr(out err)) {
            return err;
        }

        return new BaseVokiAnswerTypeData.ImageAndText(textRes.AsSuccess(), keyRes.AsSuccess());
    }


    private async Task<ErrOr<GeneralVokiAnswerImageKey>> HandleAnswerImageKey(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        string? image
    ) {
        if (image is null) {
            return ErrFactory.NoValue.Common("Image value is not provided");
        }

        if (!image.StartsWith(KeyConsts.TempFolder)) {
            return GeneralVokiAnswerImageKey.FromString(image);
        }

        ErrOr<TempImageKey> tempKeyCreationRes = TempImageKey.FromString(image);
        if (tempKeyCreationRes.IsErr(out var err)) {
            return err;
        }

        TempImageKey tempKey = tempKeyCreationRes.AsSuccess();
        ImageFileExtension ext = tempKey.Extension;
        var destination = GeneralVokiAnswerImageKey.CreateForAnswer(vokiId, questionId, ext);
        ErrOrNothing copyingRes = await _mainStorageBucket.CopyVokiAnswerImageFromTempToStandard(tempKey, destination);
        if (copyingRes.IsErr(out err)) {
            return ErrFactory.Unspecified("Couldn't save answer image", details: err.Message);
        }

        return destination;
    }


    private async Task<ErrOr<BaseVokiAnswerTypeData>> CreateAudioOnlyAnswerData(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        VokiAnswerTypeDataDto answer
    ) => (
        await HandleAnswerAudioKey(vokiId, questionId, answer.Audio)
    ).Bind<BaseVokiAnswerTypeData>(savedKey => new BaseVokiAnswerTypeData.AudioOnly(savedKey));

    private async Task<ErrOr<BaseVokiAnswerTypeData>> CreateAudioAndTextAnswerData(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        VokiAnswerTypeDataDto answer
    ) {
        ErrOr<GeneralVokiAnswerText> textRes = answer.GetText();
        if (textRes.IsErr(out var err)) {
            return err;
        }

        ErrOr<GeneralVokiAnswerAudioKey> keyRes = await HandleAnswerAudioKey(vokiId, questionId, answer.Audio);
        if (keyRes.IsErr(out err)) {
            return err;
        }

        return new BaseVokiAnswerTypeData.AudioAndText(textRes.AsSuccess(), keyRes.AsSuccess());
    }

    private async Task<ErrOr<GeneralVokiAnswerAudioKey>> HandleAnswerAudioKey(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        string? audio
    ) {
        if (audio is null) {
            return ErrFactory.NoValue.Common("Audio value is not provided");
        }

        if (!audio.StartsWith(KeyConsts.TempFolder)) {
            return GeneralVokiAnswerAudioKey.FromString(audio);
        }

        ErrOr<TempAudioKey> tempKeyCreationRes = TempAudioKey.FromString(audio);
        if (tempKeyCreationRes.IsErr(out var err)) {
            return err;
        }

        TempAudioKey tempKey = tempKeyCreationRes.AsSuccess();
        AudioFileExtension ext = tempKey.Extension;
        var destination = GeneralVokiAnswerAudioKey.CreateForAnswer(vokiId, questionId, ext);
        ErrOrNothing copyingRes = await _mainStorageBucket.CopyVokiAnswerAudioFromTempToStandard(tempKey, destination);
        if (copyingRes.IsErr(out err)) {
            return ErrFactory.Unspecified("Couldn't save answer audio", details: err.Message);
        }

        return destination;
    }
}