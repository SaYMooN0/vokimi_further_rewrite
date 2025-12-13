using SharedKernel.common.rules;
using SharedKernel.exceptions;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate;

public class VokiExpectedManagersSetting : ValueObject
{
    private bool MakeAllCoAuthorsManagers { get; }
    private ImmutableHashSet<AppUserId> UserIdsToBecomeManagers { get; }
    public override IEnumerable<object> GetEqualityComponents() => [MakeAllCoAuthorsManagers, UserIdsToBecomeManagers];

    private VokiExpectedManagersSetting(
        bool makeAllCoAuthorsManagers,
        ImmutableHashSet<AppUserId> userIdsToBecomeManagers
    ) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(userIdsToBecomeManagers));
        MakeAllCoAuthorsManagers = makeAllCoAuthorsManagers;
        UserIdsToBecomeManagers = userIdsToBecomeManagers;
    }

    private static ErrOrNothing CheckForErr(ImmutableHashSet<AppUserId> userIdsToBecomeManagers) =>
        userIdsToBecomeManagers.Count > VokiRules.MaxCoAuthors
            ? ErrFactory.Conflict("Cannot specify more users to become managers than Voki can have as co-authors")
            : ErrOrNothing.Nothing;

    public static VokiExpectedManagersSetting AllCoAuthorsWillBecomeManagers() => new(true, []);

    public static ErrOr<VokiExpectedManagersSetting> SpecifiedUsers(
        ImmutableHashSet<AppUserId> userIdsToBecomeManagers
    ) => CheckForErr(userIdsToBecomeManagers).IsErr(out var err)
        ? err
        : new VokiExpectedManagersSetting(false, userIdsToBecomeManagers);

    public VokiExpectedManagersSetting Without(AppUserId userId) => new VokiExpectedManagersSetting(
        this.MakeAllCoAuthorsManagers,
        this.UserIdsToBecomeManagers.Remove(userId)
    );

    public ImmutableHashSet<AppUserId> DecideManagers(ImmutableHashSet<AppUserId> coAuthors) =>
        MakeAllCoAuthorsManagers ? coAuthors.ToImmutableHashSet() : UserIdsToBecomeManagers.ToImmutableHashSet();

    public bool AnySpecifiedUser(Func<AppUserId, bool> predicate) =>
        this.UserIdsToBecomeManagers.Any(predicate);
    public bool SpecifiedUserContains(AppUserId userId) =>
        this.UserIdsToBecomeManagers.Contains(userId);
}