using System.Collections.Immutable;
using SharedKernel.common.rules;

namespace SharedKernel.common.vokis;

public class VokiManagersIdsSet : ValueObject
{
    private readonly ImmutableHashSet<AppUserId> _ids;

    private VokiManagersIdsSet(ImmutableHashSet<AppUserId> ids) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(ids));
        _ids = ids;
    }

    public override IEnumerable<object> GetEqualityComponents() => _ids;
    public bool Contains(AppUserId id) => _ids.Contains(id);

    public static ErrOr<VokiManagersIdsSet> Create(ImmutableHashSet<AppUserId> ids) {
        if (CheckForErr(ids).IsErr(out var err)) {
            return err;
        }

        return new VokiManagersIdsSet(ids);
    }

    private static ErrOrNothing CheckForErr(ImmutableHashSet<AppUserId> ids) =>
        ids.Count > VokiRules.MaxManagersCount
            ? ErrFactory.LimitExceeded(
                $"Voki cannot have more than {VokiRules.MaxManagersCount} managers",
                $"Provided count: {ids.Count}"
            )
            : ErrOrNothing.Nothing;

    public AppUserId[] ToArray() => _ids.ToArray();
    public ImmutableHashSet<AppUserId> ToImmutableHashSet() => _ids.ToImmutableHashSet();
}