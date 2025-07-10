using System.Collections.Immutable;
using SharedKernel.exceptions;

namespace VokiCreationServicesLib.Domain.draft_voki_aggregate;

public class VokiTagsSet : ValueObject
{
    public const int MaxTagsForVokiCount = 120;
    public ImmutableHashSet<VokiTagId> Value { get; }

    private VokiTagsSet(ImmutableHashSet<VokiTagId> value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckTagsSetForErr(value));
        Value = value;
    }

    public static ErrOr<VokiTagsSet> Create(ImmutableHashSet<VokiTagId> tags) {
        if (CheckTagsSetForErr(tags).IsErr(out var err)) {
            return err;
        }

        return new VokiTagsSet(tags);
    }

    public static ErrOrNothing CheckTagsSetForErr(ImmutableHashSet<VokiTagId> tags) {
        if (tags.Count > MaxTagsForVokiCount) {
            return ErrFactory.LimitExceeded(
                $"Too many tags selected. Voki cannot have more than {MaxTagsForVokiCount} tags",
                $"Tags selected: {tags.Count}"
            );
        }

        return ErrOrNothing.Nothing;
    }

    public override IEnumerable<object> GetEqualityComponents() => Value;
    public static VokiTagsSet Empty => new(ImmutableHashSet<VokiTagId>.Empty);
}