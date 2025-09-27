using SharedKernel.exceptions;

namespace VokiRatingsService.Domain.voki_rating_aggregate;

public class RatingValueWithDate : ValueObject
{
    private RatingValueWithDate(ushort value, DateTime dateTime) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckValueForErr(value));
        Value = value;
        DateTime = dateTime;
    }

    public ushort Value { get; }
    public DateTime DateTime { get; }
    public override IEnumerable<object> GetEqualityComponents() => [Value, DateTime];

    private const ushort
        MinValue = 1,
        MaxValue = 5;

    public static ErrOrNothing CheckValueForErr(ushort value) {
        if (value < MinValue) {
            return ErrFactory.IncorrectFormat(
                $"Rating value cannot be less than {MinValue}"
            );
        }

        if (value > MaxValue) {
            return ErrFactory.IncorrectFormat(
                $"Rating value cannot be greater than {MaxValue}"
            );
        }

        return ErrOrNothing.Nothing;
    }


    public static ErrOr<RatingValueWithDate> Create(ushort value, DateTime dateTime) =>
        CheckValueForErr(value).IsErr(out var err) ? err : new RatingValueWithDate(value, dateTime);
}