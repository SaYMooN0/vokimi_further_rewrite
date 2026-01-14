using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.extension;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.answers.auxiliary;

public class DraftVokiAnswerDataSavingService
{
    private readonly IMainStorageBucket _mainStorageBucket;

    public DraftVokiAnswerDataSavingService(IMainStorageBucket mainStorageBucket) {
        _mainStorageBucket = mainStorageBucket;
    }

    public Task<ErrOr<BaseVokiAnswerTypeData>> SaveAnswerData(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        VokiAnswerTypeDataDto d,
        CancellationToken ct
    ) => d.Type.Match(
        textOnly: () => Task.FromResult(
            d.GetText().Bind<BaseVokiAnswerTypeData>(text => new BaseVokiAnswerTypeData.TextOnly(text))
        ),
        imageOnly: () => CreateImageOnlyAnswerData(vokiId, questionId, d, ct),
        imageAndText: () => CreateImageAndTextAnswerData(vokiId, questionId, d, ct),
        colorOnly: () => Task.FromResult(
            d.GetColor().Bind<BaseVokiAnswerTypeData>(color => new BaseVokiAnswerTypeData.ColorOnly(color))),
        colorAndText: () => Task.FromResult(
            d.GetText().Bind<BaseVokiAnswerTypeData>(text =>
                d.GetColor().Bind<BaseVokiAnswerTypeData>(color => new BaseVokiAnswerTypeData.ColorAndText(text, color))
            )),
        audioOnly: () => CreateAudioOnlyAnswerData(vokiId, questionId, d, ct),
        audioAndText: () => CreateAudioAndTextAnswerData(vokiId, questionId, d, ct)
    );


    private async Task<ErrOr<BaseVokiAnswerTypeData>> CreateImageOnlyAnswerData(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        VokiAnswerTypeDataDto answer,
        CancellationToken ct
    ) => (
        await HandleAnswerImageKey(vokiId, questionId, answer.Image, ct)
    ).Bind<BaseVokiAnswerTypeData>(savedKey => new BaseVokiAnswerTypeData.ImageOnly(savedKey));

    private async Task<ErrOr<BaseVokiAnswerTypeData>> CreateImageAndTextAnswerData(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        VokiAnswerTypeDataDto answer,
        CancellationToken ct
    ) {
        ErrOr<GeneralVokiAnswerText> textRes = answer.GetText();
        if (textRes.IsErr(out var err)) {
            return err;
        }

        ErrOr<GeneralVokiAnswerImageKey> keyRes = await HandleAnswerImageKey(vokiId, questionId, answer.Image, ct);
        if (keyRes.IsErr(out err)) {
            return err;
        }

        return new BaseVokiAnswerTypeData.ImageAndText(textRes.AsSuccess(), keyRes.AsSuccess());
    }


    private async Task<ErrOr<GeneralVokiAnswerImageKey>> HandleAnswerImageKey(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        string? image,
        CancellationToken ct
    ) {
        if (image is null) {
            return ErrFactory.NoValue.Common("Image value is not provided");
        }

        if (GeneralVokiAnswerImageKey.FromString(image).IsSuccess(out var savedKey)) {
            return savedKey;
        }

        ErrOr<TempImageKey> tempKeyCreationRes = TempImageKey.FromString(image);
        if (tempKeyCreationRes.IsErr(out var err)) {
            return err;
        }

        TempImageKey tempKey = tempKeyCreationRes.AsSuccess();
        ImageFileExtension ext = tempKey.Extension;
        var destination = GeneralVokiAnswerImageKey.CreateForAnswer(vokiId, questionId, ext);
        ErrOrNothing copyingRes =
            await _mainStorageBucket.CopyVokiAnswerImageFromTempToStandard(tempKey, destination, ct);
        if (copyingRes.IsErr(out err)) {
            return ErrFactory.Unspecified("Couldn't save answer image", details: err.Message);
        }

        return destination;
    }


    private async Task<ErrOr<BaseVokiAnswerTypeData>> CreateAudioOnlyAnswerData(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        VokiAnswerTypeDataDto answer,
        CancellationToken ct
    ) => (
        await HandleAnswerAudioKey(vokiId, questionId, answer.Audio, ct)
    ).Bind<BaseVokiAnswerTypeData>(savedKey => new BaseVokiAnswerTypeData.AudioOnly(savedKey));

    private async Task<ErrOr<BaseVokiAnswerTypeData>> CreateAudioAndTextAnswerData(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        VokiAnswerTypeDataDto answer,
        CancellationToken ct
    ) {
        ErrOr<GeneralVokiAnswerText> textRes = answer.GetText();
        if (textRes.IsErr(out var err)) {
            return err;
        }

        ErrOr<GeneralVokiAnswerAudioKey> keyRes = await HandleAnswerAudioKey(vokiId, questionId, answer.Audio, ct);
        if (keyRes.IsErr(out err)) {
            return err;
        }

        return new BaseVokiAnswerTypeData.AudioAndText(textRes.AsSuccess(), keyRes.AsSuccess());
    }

    private async Task<ErrOr<GeneralVokiAnswerAudioKey>> HandleAnswerAudioKey(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        string? audio,
        CancellationToken ct
    ) {
        if (audio is null) {
            return ErrFactory.NoValue.Common("Audio value is not provided");
        }


        if (GeneralVokiAnswerAudioKey.FromString(audio).IsSuccess(out var savedKey)) {
            return savedKey;
        }

        ErrOr<TempAudioKey> tempKeyCreationRes = TempAudioKey.FromString(audio);
        if (tempKeyCreationRes.IsErr(out var err)) {
            return err;
        }

        TempAudioKey tempKey = tempKeyCreationRes.AsSuccess();
        AudioFileExtension ext = tempKey.Extension;
        GeneralVokiAnswerAudioKey destination = GeneralVokiAnswerAudioKey.CreateForAnswer(vokiId, questionId, ext);
        ErrOrNothing copyingRes =
            await _mainStorageBucket.CopyVokiAnswerAudioFromTempToStandard(tempKey, destination, ct);
        if (copyingRes.IsErr(out err)) {
            return ErrFactory.Unspecified("Couldn't save answer audio", details: err.Message);
        }

        return destination;
    }
}