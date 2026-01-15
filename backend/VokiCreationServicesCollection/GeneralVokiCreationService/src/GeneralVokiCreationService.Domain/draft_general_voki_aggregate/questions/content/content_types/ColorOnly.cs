using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;


public abstract partial record BaseQuestionTypeSpecificContent
{
    public sealed record ColorOnly(
        QuestionAnswersList<BaseQuestionAnswer.ColorOnly> Answers
    ) : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiAnswerType AnswersType => GeneralVokiAnswerType.ColorOnly;
        public override IEnumerable<BaseQuestionAnswer> BaseAnswers => Answers.AsIEnumerable;


        public override BaseQuestionTypeSpecificContent RemoveResult(GeneralVokiResultId resultId) => new ColorOnly(
            Answers: Answers.ApplyForEach(a => (BaseQuestionAnswer.ColorOnly)a.RemoveRelatedResult(resultId))
        );

        public static ColorOnly Empty() => new(
            Answers: QuestionAnswersList<BaseQuestionAnswer.ColorOnly>.Empty()
        );
    }
}