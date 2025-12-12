namespace CoreVokiCreationService.Domain.draft_voki_aggregate;

public class VokiExpectedManagersSetting : ValueObject
{
    public bool MakeAllCoAuthorsManagers { get; }
    public ImmutableHashSet<AppUserId> UserIdsToBecomeManagers { get; }
    public override IEnumerable<object> GetEqualityComponents() => [MakeAllCoAuthorsManagers, UserIdsToBecomeManagers];
}