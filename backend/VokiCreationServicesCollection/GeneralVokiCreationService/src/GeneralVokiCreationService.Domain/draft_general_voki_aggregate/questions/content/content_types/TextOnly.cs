using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;

public abstract partial record BaseQuestionTypeSpecificContent
{
    public sealed record TextOnly(
        QuestionAnswersList<BaseQuestionAnswer.TextOnly> Answers
    ) : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiAnswerType AnswersType => GeneralVokiAnswerType.TextOnly;
        public override IEnumerable<BaseQuestionAnswer> BaseAnswers => Answers.AsIEnumerable;


        public override BaseQuestionTypeSpecificContent RemoveResult(GeneralVokiResultId resultId) => new TextOnly(
            Answers: Answers.ApplyForEach(a => (BaseQuestionAnswer.TextOnly)a.RemoveRelatedResult(resultId))
        );

        public static TextOnly Empty() => new(
            Answers: QuestionAnswersList<BaseQuestionAnswer.TextOnly>.Empty()
        );
    }
}