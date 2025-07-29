using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Api.contracts.answers;

public class AddNewAnswerToVokiQuestionRequest : IRequestWithValidationNeeded
{
    public VokiAnswerTypeDataDto AnswerData { get; init; }
    public GeneralVokiAnswerType AnswersType { get; init; }
    public  BaseVokiAnswerTypeData ParsedAnswerData { get; private set; }

    public ErrOrNothing Validate() {
        if (AnswerData is null) {
            return ErrFactory.NoValue.Common($"{nameof(AnswerData)} is not provided");
        }

        if (AnswerData.IsEmpty()) {
            return ErrFactory.NoValue.Common($"{nameof(AnswerData)} is empty");
        }

        var parseRes = AnswerData.ParseToAnswerData(AnswersType);
        if (parseRes.IsErr(out var err)) {
            return err;
        }

        ParsedAnswerData = parseRes.AsSuccess();
        return ErrOrNothing.Nothing;
    }
}