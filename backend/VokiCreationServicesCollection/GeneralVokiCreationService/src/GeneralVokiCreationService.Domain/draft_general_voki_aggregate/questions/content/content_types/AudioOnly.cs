using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;


public abstract partial record BaseQuestionTypeSpecificContent
{
    public sealed record AudioOnly(
        QuestionAnswersList<BaseQuestionAnswer.AudioOnly> Answers
    )
        : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiAnswerType AnswersType => GeneralVokiAnswerType.AudioOnly;
        public override IEnumerable<BaseQuestionAnswer> BaseAnswers => Answers.AsIEnumerable;

        public override BaseQuestionTypeSpecificContent RemoveResult(GeneralVokiResultId resultId) => new AudioOnly(
            Answers: Answers.ApplyForEach(a => (BaseQuestionAnswer.AudioOnly)a.RemoveRelatedResult(resultId))
        );

        public static AudioOnly Empty() => new(
            Answers: QuestionAnswersList<BaseQuestionAnswer.AudioOnly>.Empty()
        );
    }
    
}