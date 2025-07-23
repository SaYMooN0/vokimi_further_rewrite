using GeneralVokiCreationService.Application.draft_vokis;

namespace GeneralVokiCreationService.Api.contracts.answers;

public class AddNewAnswerToVokiQuestionRequest : IRequestWithValidationNeeded
{
    public VokiAnswerTypeDataDto VokiAnswerTypeData { get; init; }
    public ErrOrNothing Validate() => throw new NotImplementedException();
}