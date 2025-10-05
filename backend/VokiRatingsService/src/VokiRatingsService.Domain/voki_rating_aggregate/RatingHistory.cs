namespace VokiRatingsService.Domain.voki_rating_aggregate;

public class RatingHistory : Entity<RatingHistoryId>
{
    private RatingHistory() { }

    public ImmutableArray<RatingValueWithDate> Values { get; private set; }

    private RatingHistory(RatingHistoryId id, ImmutableArray<RatingValueWithDate> previousValues) {
        Id = id;
        Values = previousValues;
    }


    public static RatingHistory CreateNew() => new(RatingHistoryId.CreateNew(), []);

    public ErrOrNothing Add(RatingValueWithDate newVal) {
        if (Values.Length == 0) {
            Values = [newVal];
            return ErrOrNothing.Nothing;
        }

        var last = Values.MaxBy(vr => vr.DateTime)!;
        if (newVal.DateTime < last.DateTime) {
            return ErrFactory.ValueOutOfRange(
                "Date of the new rating added to a history list earlier than the last date",
                $"New value datetime:{newVal.DateTime:o}. Last date: {last.DateTime:o}"
            );
        }

        Values = Values.Add(newVal);
        return ErrOrNothing.Nothing;
    }
}