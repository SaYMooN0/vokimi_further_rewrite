using SharedKernel.common.rules;

namespace VokiCreationServicesLib.Api.contracts.voki_publishing;

public class PublishVokiRequest : IRequestWithValidationNeeded
{
    public string[] CoAuthorIdsToPublishWith { get; init; }

    public ErrOrNothing Validate() {
        if (CoAuthorIdsToPublishWith.Length > VokiRules.MaxCoAuthors) {
            return ErrFactory.LimitExceeded(
                $"Too many users to become co-authors are selected. Maximum number is: {VokiRules.MaxCoAuthors}",
                $"You have selected: {CoAuthorIdsToPublishWith.Length}"
            );
        }

        string[] incorrectVals = CoAuthorIdsToPublishWith
            .Where(u => !Guid.TryParse(u, out _))
            .ToArray();
        if (incorrectVals.Length > 0) {
            return ErrFactory.IncorrectFormat(
                $"Some of the selected users({incorrectVals.Length}) could not be identified",
                $"Incorrect user ids: {string.Join(", ", incorrectVals)}"
            );
        }

        ParsedCoAuthorIds = CoAuthorIdsToPublishWith
            .Select(v => new AppUserId(new(v)))
            .ToHashSet();
        return ErrOrNothing.Nothing;
    }

    public ISet<AppUserId> ParsedCoAuthorIds { get; private set; }
}