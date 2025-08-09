namespace SharedKernel.common.vokis;

public enum GeneralVokiAnswerType
{
    TextOnly,
    ImageOnly,
    ImageAndText,
    ColorOnly,
    ColorAndText,
    AudioOnly,
    AudioAndText
}

public static class GeneralVokiAnswerTypeExtensions
{
    public static T Match<T>(
        this GeneralVokiAnswerType type,
        Func<T> textOnly,
        Func<T> imageOnly, Func<T> imageAndText,
        Func<T> colorOnly, Func<T> colorAndText,
        Func<T> audioOnly, Func<T> audioAndText
    ) => type switch {
        GeneralVokiAnswerType.TextOnly => textOnly(),
        GeneralVokiAnswerType.ImageOnly => imageOnly(),
        GeneralVokiAnswerType.ImageAndText => imageAndText(),
        GeneralVokiAnswerType.ColorOnly => colorOnly(),
        GeneralVokiAnswerType.ColorAndText => colorAndText(),
        GeneralVokiAnswerType.AudioOnly => audioOnly(),
        GeneralVokiAnswerType.AudioAndText => audioAndText(),
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };
    public static bool HasAudio(this GeneralVokiAnswerType answersType) => answersType switch {
        GeneralVokiAnswerType.AudioOnly => true,
        GeneralVokiAnswerType.AudioAndText => true,
        _ => false
    };
}