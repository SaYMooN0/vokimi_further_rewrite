using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Api.contracts.answers;

public class AddNewAnswerToVokiQuestionRequest : IRequestWithValidationNeeded
{
    public VokiAnswerTypeDataDto AnswerData { get; init; }
    public GeneralVokiAnswerType AnswersType { get; init; }
    public string[] RelateResultIds { get; init; }
    private const int MaxRelatedResultsCount = 120;

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

        if (RelateResultIds.Length > MaxRelatedResultsCount) {
            return ErrFactory.LimitExceeded("Too many related results provided");
        }

        var incorrectResultIds = RelateResultIds.Where(id => !Guid.TryParse(id, out _)).ToArray();
        if (incorrectResultIds.Length > 0) {
            return ErrFactory.IncorrectFormat(
                "Some of the provided related result ids are invalid",
                $"Result ids {string.Join(", ", incorrectResultIds)} are incorrect result ids"
            );
        }

        ParsedAnswerData = parseRes.AsSuccess();
        return ErrOrNothing.Nothing;
    }

    public BaseVokiAnswerTypeData ParsedAnswerData { get; private set; }

    public ImmutableHashSet<GeneralVokiResultId> ParsedResultIds => RelateResultIds
        .Select(id => new GeneralVokiResultId(new(id)))
        .ToImmutableHashSet();
}