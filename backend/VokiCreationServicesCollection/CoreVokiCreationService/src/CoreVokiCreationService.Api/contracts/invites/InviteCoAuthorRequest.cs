namespace CoreVokiCreationService.Api.contracts.invites;

public class InviteCoAuthorRequest : IRequestWithValidationNeeded
{
    public string NewCoAuthorId { get; init; } = "";

    public ErrOrNothing Validate() =>
        Guid.TryParse(NewCoAuthorId, out var _)
            ? ErrOrNothing.Nothing
            : ErrFactory.IncorrectFormat(
                "Incorrect new co-author id format",
                $"Given value: {NewCoAuthorId} is not in a correct format for user id"
            );

    public AppUserId ParsedNewCoAuthorId => new AppUserId(new Guid(NewCoAuthorId));
}