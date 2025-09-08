using GeneralVokiCreationService.Application.draft_vokis.commands.answers.auxiliary;

namespace GeneralVokiCreationService.Api.contracts.answers;

public class SaveVokiQuestionAnswerRequest : IRequestWithValidationNeeded
{
    public VokiAnswerTypeDataDto AnswerData { get; init; }
    public string[] RelateResultIds { get; init; }
    private const int MaxRelatedResultsCount = 120;

    public ErrOrNothing Validate() {
        if (AnswerData is null) {
            return ErrFactory.NoValue.Common($"{nameof(AnswerData)} is not provided");
        }

        if (AnswerData.IsEmpty()) {
            return ErrFactory.NoValue.Common($"{nameof(AnswerData)} is empty");
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

        return ErrOrNothing.Nothing;
    }

    public ImmutableHashSet<GeneralVokiResultId> ParsedResultIds => RelateResultIds
        .Select(id => new GeneralVokiResultId(new(id)))
        .ToImmutableHashSet();
}