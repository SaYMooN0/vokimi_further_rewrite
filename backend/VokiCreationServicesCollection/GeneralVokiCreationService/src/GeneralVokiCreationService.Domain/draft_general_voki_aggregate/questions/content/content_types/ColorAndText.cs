using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;


public abstract partial record BaseQuestionTypeSpecificContent
{
    public sealed record ColorAndText(
        QuestionAnswersList<BaseQuestionAnswer.ColorAndText> Answers
    ) : BaseQuestionTypeSpecificContent
    {
        public override GeneralVokiQuestionContentType Type => GeneralVokiQuestionContentType.ColorAndText;
        public override IEnumerable<BaseQuestionAnswer> BaseAnswers => Answers.AsIEnumerable;


        public override BaseQuestionTypeSpecificContent RemoveResult(GeneralVokiResultId resultId) => new ColorAndText(
            Answers: Answers.ApplyForEach(a => (BaseQuestionAnswer.ColorAndText)a.RemoveRelatedResult(resultId))
        );

        public static ColorAndText Empty() => new(
            Answers: QuestionAnswersList<BaseQuestionAnswer.ColorAndText>.Empty()
        );
    }
    
}