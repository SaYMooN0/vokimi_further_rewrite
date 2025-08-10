using SharedKernel.exceptions;
using VokimiStorageKeysLib.general_voki.question_image;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

public class VokiQuestionImagesSet : ValueObject

{
    public const int MaxImagesForQuestionCount = 5;
    public  ImmutableArray<GeneralVokiQuestionImageKey> Keys { get; }

    private VokiQuestionImagesSet(ImmutableArray<GeneralVokiQuestionImageKey> keys) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(keys));
        this.Keys = keys;
    }

    public static VokiQuestionImagesSet Empty => new(ImmutableArray<GeneralVokiQuestionImageKey>.Empty);

    public override IEnumerable<object> GetEqualityComponents() => Keys;

    public static ErrOr<VokiQuestionImagesSet> Create(ImmutableArray<GeneralVokiQuestionImageKey> keys) =>
        CheckForErr(keys).IsErr(out var err) ? err : new VokiQuestionImagesSet(keys);

    public static ErrOrNothing CheckForErr(ImmutableArray<GeneralVokiQuestionImageKey> keys) =>
        keys.Length > MaxImagesForQuestionCount
            ? ErrFactory.LimitExceeded(
                $"A question can have at most {MaxImagesForQuestionCount} images",
                $"Current count is {keys.Length}")
            : ErrOrNothing.Nothing;
}