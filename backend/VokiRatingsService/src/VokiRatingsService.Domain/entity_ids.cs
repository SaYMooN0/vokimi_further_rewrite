namespace VokiRatingsService.Domain;

public class RatingHistoryId(Guid value) : GuidBasedId(value)
{
    public static RatingHistoryId CreateNew() => new(Guid.CreateVersion7());
}
