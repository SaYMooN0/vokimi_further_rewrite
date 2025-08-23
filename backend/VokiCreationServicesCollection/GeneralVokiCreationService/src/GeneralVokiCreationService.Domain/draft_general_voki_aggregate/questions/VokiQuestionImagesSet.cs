using SharedKernel.exceptions;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

public class VokiQuestionImagesSet : ValueObject

{
    public const int MaxImagesForQuestionCount = 5;
    public ImmutableArray<GeneralVokiQuestionImageKey> Keys { get; }

    private VokiQuestionImagesSet(ImmutableArray<GeneralVokiQuestionImageKey> keys) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(keys.Length));
        this.Keys = keys;
    }

    public static VokiQuestionImagesSet Empty => new(ImmutableArray<GeneralVokiQuestionImageKey>.Empty);

    public override IEnumerable<object> GetEqualityComponents() => Keys;

    public static ErrOr<VokiQuestionImagesSet> Create(ImmutableArray<GeneralVokiQuestionImageKey> keys) =>
        CheckForErr(keys.Length).IsErr(out var err) ? err : new VokiQuestionImagesSet(keys);

    public static ErrOrNothing CheckForErr(int count) =>
        count > MaxImagesForQuestionCount
            ? ErrFactory.LimitExceeded(
                $"A question can have at most {MaxImagesForQuestionCount} images",
                $"Current count is {count}")
            : ErrOrNothing.Nothing;
}