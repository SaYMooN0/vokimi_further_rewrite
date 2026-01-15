using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;

public abstract partial record BaseQuestionTypeSpecificContent
{
    public abstract GeneralVokiAnswerType AnswersType { get; }
    public abstract IEnumerable<BaseQuestionAnswer> BaseAnswers { get; }
    [Pure]
    public abstract BaseQuestionTypeSpecificContent RemoveResult(GeneralVokiResultId resultId);
    public TResult Match<TResult>(
        Func<TextOnly, TResult> textOnly,
        Func<ImageOnly, TResult> imageOnly,
        Func<ImageAndText, TResult> imageAndText,
        Func<ColorOnly, TResult> colorOnly,
        Func<ColorAndText, TResult> colorAndText,
        Func<AudioOnly, TResult> audioOnly,
        Func<AudioAndText, TResult> audioAndText
    ) => AnswersType.Match(
        textOnly: () => textOnly((TextOnly)this),
        imageOnly: () => imageOnly((ImageOnly)this),
        imageAndText: () => imageAndText((ImageAndText)this),
        colorOnly: () => colorOnly((ColorOnly)this),
        colorAndText: () => colorAndText((ColorAndText)this),
        audioOnly: () => audioOnly((AudioOnly)this),
        audioAndText: () => audioAndText((AudioAndText)this)
    );

    public static BaseQuestionTypeSpecificContent CreateEmpty(GeneralVokiAnswerType t) =>
        t.Match<BaseQuestionTypeSpecificContent>(
            textOnly: TextOnly.Empty,
            imageOnly: ImageOnly.Empty,
            imageAndText: ImageAndText.Empty,
            colorOnly: ColorOnly.Empty,
            colorAndText: ColorAndText.Empty,
            audioOnly: AudioOnly.Empty,
            audioAndText: AudioAndText.Empty
        );
}

public interface IContentWithStorageKey
{
    public bool IsForCorrectVokiQuestion(VokiId vokiId, GeneralVokiQuestionId questionId);
}