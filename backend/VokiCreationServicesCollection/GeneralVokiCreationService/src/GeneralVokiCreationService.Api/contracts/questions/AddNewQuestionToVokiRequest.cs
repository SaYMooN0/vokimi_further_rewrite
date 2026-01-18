using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiCreationService.Api.contracts.questions;

public class AddNewQuestionToVokiRequest : IRequestWithValidationNeeded
{
    public  GeneralVokiAnswerType QuestionAnswersType { get; init; }
    public ErrOrNothing Validate() => ErrOrNothing.Nothing;
}