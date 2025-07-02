using System.Collections.Immutable;
using SharedKernel.exceptions;
using SharedKernel.rules;

namespace GeneralVokiCreationService.Domain.draft_voki_aggregate;

public class VokiCoAuthorIdsSet : ValueObject
{
    public ImmutableHashSet<AppUserId> Ids { get; }

    public VokiCoAuthorIdsSet(ImmutableHashSet<AppUserId> ids) {
        InvalidConstructorArgumentException.ThrowIfErr(CheckForErr(ids));
        Ids = ids;
    }
    public override IEnumerable<object> GetEqualityComponents() => Ids;

    public static VokiCoAuthorIdsSet Empty => new(ImmutableHashSet<AppUserId>.Empty);
    public bool Contains(AppUserId id) => Ids.Contains(id);

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

}