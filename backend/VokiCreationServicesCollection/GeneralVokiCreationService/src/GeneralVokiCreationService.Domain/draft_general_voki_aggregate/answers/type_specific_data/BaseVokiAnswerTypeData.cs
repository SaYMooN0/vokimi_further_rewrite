namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;

public abstract partial record BaseVokiAnswerTypeData
{
    public abstract GeneralVokiAnswerType MatchingEnum { get; }

    public TResult Match<TResult>(
        Func<TextOnly, TResult> textOnly,
        Func<ImageOnly, TResult> imageOnly,
        Func<ImageAndText, TResult> imageAndText,
        Func<ColorOnly, TResult> colorOnly,
        Func<ColorAndText, TResult> colorAndText,
        Func<AudioOnly, TResult> audioOnly,
        Func<AudioAndText, TResult> audioAndText
    ) => MatchingEnum.Match(
        textOnly: () => textOnly((TextOnly)this),
        imageOnly: () => imageOnly((ImageOnly)this),
        imageAndText: () => imageAndText((ImageAndText)this),
        colorOnly: () => colorOnly((ColorOnly)this),
        colorAndText: () => colorAndText((ColorAndText)this),
        audioOnly: () => audioOnly((AudioOnly)this),
        audioAndText: () => audioAndText((AudioAndText)this)
    );
}

public interface IVokiAnswerTypeDataWithStorageKey
{
    public bool IsForCorrectVokiQuestion(
        VokiId vokiId,
        GeneralVokiQuestionId questionId,
        GeneralVokiAnswerId answerId
    );
}