using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.answers.answer_types;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions.content.content_types;


public abstract partial record BaseQuestionTypeSpecificContent
{
    public sealed record ImageAndText(
        QuestionAnswersList<BaseQuestionAnswer.ImageAndText> Answers
    ) : BaseQuestionTypeSpecificContent, IContentWithStorageKeys
    {
        public override GeneralVokiQuestionContentType Type => GeneralVokiQuestionContentType.ImageAndText;
        public override IEnumerable<BaseQuestionAnswer> BaseAnswers => Answers.AsIEnumerable;

        public override BaseQuestionTypeSpecificContent RemoveResult(GeneralVokiResultId resultId) => new ImageAndText(
            Answers: Answers.ApplyForEach(a => (BaseQuestionAnswer.ImageAndText)a.RemoveRelatedResult(resultId))
        );
        public static ImageAndText Empty() => new(
            Answers: QuestionAnswersList<BaseQuestionAnswer.ImageAndText>.Empty()
        );
        public bool IsAllForCorrectVokiQuestion(VokiId vokiId, GeneralVokiQuestionId questionId) =>
            Answers.All(a => a.IsForCorrectVokiQuestion(vokiId, questionId));
    }

}