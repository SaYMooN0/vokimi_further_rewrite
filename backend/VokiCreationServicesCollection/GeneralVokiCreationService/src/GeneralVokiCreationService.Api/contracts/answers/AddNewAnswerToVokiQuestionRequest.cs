using GeneralVokiCreationService.Application.draft_vokis;

namespace GeneralVokiCreationService.Api.contracts.answers;

public class AddNewAnswerToVokiQuestionRequest : IRequestWithValidationNeeded
{
    public VokiAnswerTypeDataDto VokiAnswerTypeData { get; init; }

    public ErrOrNothing Validate() =>
        VokiAnswerTypeData is null ? ErrFactory.NoValue.Common($"{nameof(VokiAnswerTypeData)} is not provided") :
        VokiAnswerTypeData.IsEmpty() ? ErrFactory.NoValue.Common($"{nameof(VokiAnswerTypeData)} is empty") :
        ErrOrNothing.Nothing;
}