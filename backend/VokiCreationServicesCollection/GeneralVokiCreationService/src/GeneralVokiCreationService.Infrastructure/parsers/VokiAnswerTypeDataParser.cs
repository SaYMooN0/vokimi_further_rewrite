using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using SharedKernel.common.vokis;
using SharedKernel.errs.utils;

namespace GeneralVokiCreationService.Infrastructure.parsers;

public static class VokiAnswerTypeDataParser
{
    public static ErrOr<BaseVokiAnswerTypeData> CreateFromDictionary(
        GeneralVokiAnswerType type,
        Dictionary<string, string> dictionary
    ) => type.Match(
        textOnly:      () => CreateTextOnly(dictionary),
        imageOnly:     () => CreateImageOnly(dictionary),
        imageAndText:  () => CreateImageAndText(dictionary),
        colorOnly:     () => CreateColorOnly(dictionary),
        colorAndText:  () => CreateColorAndText(dictionary),
        audioOnly:     () => CreateAudioOnly(dictionary),
        audioAndText:  () => CreateAudioAndText(dictionary)
    );

    public static ErrOr<BaseVokiAnswerTypeData> CreateTextOnly(Dictionary<string, string> dictionary) {
        if (!dictionary.TryGetValue("Text", out string text)) {
            return ErrFactory.NoValue.Common("Unable to create text only answer data. Text field not provided");
        }

        var result = BaseVokiAnswerTypeData.TextOnly.CreateNew(text);
        if (result.IsErr(out var err)) {
            return err;
        }

        return result.AsSuccess();
    }
    private static ErrOr<BaseVokiAnswerTypeData> CreateImageOnly(Dictionary<string, string> dictionary) {
        throw new NotImplementedException();
    }
    private static ErrOr<BaseVokiAnswerTypeData> CreateImageAndText(Dictionary<string, string> dictionary) {
        throw new NotImplementedException();
    }
    private static ErrOr<BaseVokiAnswerTypeData> CreateColorOnly(Dictionary<string, string> dictionary) {
        throw new NotImplementedException();
    }
    private static ErrOr<BaseVokiAnswerTypeData> CreateColorAndText(Dictionary<string, string> dictionary) {
        throw new NotImplementedException();
    }
    private static ErrOr<BaseVokiAnswerTypeData> CreateAudioOnly(Dictionary<string, string> dictionary) {
        throw new NotImplementedException();
    }
    private static ErrOr<BaseVokiAnswerTypeData> CreateAudioAndText(Dictionary<string, string> dictionary) {
        throw new NotImplementedException();
    }
}