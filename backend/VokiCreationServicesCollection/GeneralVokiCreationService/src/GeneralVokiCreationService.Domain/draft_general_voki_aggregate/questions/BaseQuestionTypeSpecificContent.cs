using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

public abstract record BaseQuestionTypeSpecificContent
{
    public abstract GeneralVokiAnswerType AnswersType { get; }

    private TResult Match<TResult>(
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

    public static BaseQuestionTypeSpecificContent Empty(GeneralVokiAnswerType t) =>
        t.Match<BaseQuestionTypeSpecificContent>(
            textOnly: () => new TextOnly(QuestionAnswersList<BaseQuestionAnswer.TextOnly>.Empty()),
            imageOnly: () => new ImageOnly(QuestionAnswersList<BaseQuestionAnswer.ImageOnly>.Empty()),
            imageAndText: () => new ImageAndText(QuestionAnswersList<BaseQuestionAnswer.ImageAndText>.Empty()),
            colorOnly: () => new ColorOnly(QuestionAnswersList<BaseQuestionAnswer.ColorOnly>.Empty()),
            colorAndText: () => new ColorAndText(QuestionAnswersList<BaseQuestionAnswer.ColorAndText>.Empty()),
            audioOnly: () => new AudioOnly(QuestionAnswersList<BaseQuestionAnswer.AudioOnly>.Empty()),
            audioAndText: () => new AudioAndText(QuestionAnswersList<BaseQuestionAnswer.AudioAndText>.Empty())
        );

    public sealed record TextOnly(
        QuestionAnswersList<BaseQuestionAnswer.TextOnly> Answers
    ) : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiAnswerType AnswersType => GeneralVokiAnswerType.TextOnly;
    }

    public sealed record ImageOnly(
        QuestionAnswersList<BaseQuestionAnswer.ImageOnly> Answers
    ) : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiAnswerType AnswersType => GeneralVokiAnswerType.ImageOnly;
    }

    public sealed record ImageAndText(
        QuestionAnswersList<BaseQuestionAnswer.ImageAndText> Answers
    ) : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiAnswerType AnswersType => GeneralVokiAnswerType.ImageAndText;
    }


    public sealed record ColorOnly(
        QuestionAnswersList<BaseQuestionAnswer.ColorOnly> Answers
    ) : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiAnswerType AnswersType => GeneralVokiAnswerType.ColorOnly;
    }

    public sealed record ColorAndText(
        QuestionAnswersList<BaseQuestionAnswer.ColorAndText> Answers
    ) : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiAnswerType AnswersType => GeneralVokiAnswerType.ColorAndText;
    }


    public sealed record AudioOnly(
        QuestionAnswersList<BaseQuestionAnswer.AudioOnly> Answers
    )
        : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiAnswerType AnswersType => GeneralVokiAnswerType.AudioOnly;
    }

    public sealed record AudioAndText(
        QuestionAnswersList<BaseQuestionAnswer.AudioAndText> Answers
    )
        : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiAnswerType AnswersType => GeneralVokiAnswerType.AudioAndText;
    }
}