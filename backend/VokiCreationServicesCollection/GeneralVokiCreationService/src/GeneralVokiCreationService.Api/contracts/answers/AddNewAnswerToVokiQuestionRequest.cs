using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Api.contracts.answers;

public class AddNewAnswerToVokiQuestionRequest : IRequestWithValidationNeeded
{
    public VokiAnswerTypeDataDto VokiAnswerTypeData { get; init; }
    public GeneralVokiAnswerType AnswersType { get; init; }
    public  BaseVokiAnswerTypeData ParsedAnswerData { get; private set; }

    public ErrOrNothing Validate() {
        if (VokiAnswerTypeData is null) {
            return ErrFactory.NoValue.Common($"{nameof(VokiAnswerTypeData)} is not provided");
        }

        if (VokiAnswerTypeData.IsEmpty()) {
            return ErrFactory.NoValue.Common($"{nameof(VokiAnswerTypeData)} is empty");
        }

        var parseRes = VokiAnswerTypeData.ParseToAnswerData(AnswersType);
        if (parseRes.IsErr(out var err)) {
            return err;
        }

        ParsedAnswerData = parseRes.AsSuccess();
        return ErrOrNothing.Nothing;
    }
}