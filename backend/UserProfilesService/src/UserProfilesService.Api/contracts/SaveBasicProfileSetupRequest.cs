namespace UserProfilesService.Api.contracts;

public class SaveBasicProfileSetupRequest : IRequestWithValidationNeeded
{
    public string ProfilePic { get; init; }
    public string Username { get; init; }
    public Language[] PreferredLanguages { get; init; }
    public string[] Tags { get; init; }
    public ErrOrNothing Validate() => throw new NotImplementedException();
}