namespace CoreVokiCreationService.Api.contracts.invites;

public class CancelCoAuthorInviteRequest : IRequestWithValidationNeeded
{
    public string CoAuthorId { get; init; } = "";

    public ErrOrNothing Validate() =>
        Guid.TryParse(CoAuthorId, out var _)
            ? ErrOrNothing.Nothing
            : ErrFactory.IncorrectFormat(
                "Incorrect new co-author id format",
                $"Given value: {CoAuthorId} is not in a correct format for user id"
            );

    public AppUserId ParsedCoAuthorId => new AppUserId(new Guid(CoAuthorId));
}