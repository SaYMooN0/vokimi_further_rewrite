using System.Collections.Immutable;
using SharedKernel.common.rules;
using SharedKernel.exceptions;

namespace VokiCreationServicesLib.Domain.draft_voki_aggregate;

public class VokiCoAuthorIdsSet : ValueObject
{
    private readonly ImmutableHashSet<AppUserId> _ids;

    private VokiCoAuthorIdsSet(ImmutableHashSet<AppUserId> ids) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(ids));
        _ids = ids;
    }

    public override IEnumerable<object> GetEqualityComponents() => _ids;

    public static VokiCoAuthorIdsSet Empty => new(ImmutableHashSet<AppUserId>.Empty);
    public bool Contains(AppUserId id) => _ids.Contains(id);
    public int Count => _ids.Count;

    public static ErrOr<VokiCoAuthorIdsSet> Create(ImmutableHashSet<AppUserId> ids) {
        if (CheckForErr(ids).IsErr(out var err)) {
            return err;
        }

        return new VokiCoAuthorIdsSet(ids);
    }

    public static ErrOrNothing CheckForErr(ImmutableHashSet<AppUserId> ids) =>
        ids is null ? ErrFactory.NoValue.Common("Co-author list is null")
        : ids.Count > VokiRules.MaxCoAuthors ? ErrFactory.LimitExceeded(
            $"A Voki can have at most {VokiRules.MaxCoAuthors} co-authors",
            $"Current count is {ids.Count}")
        : ErrOrNothing.Nothing;

    public AppUserId[] ToArray() => _ids.ToArray();
}