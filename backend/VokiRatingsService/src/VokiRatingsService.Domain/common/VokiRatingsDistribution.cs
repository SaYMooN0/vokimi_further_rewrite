namespace VokiRatingsService.Domain.voki_ratings_snapshot;

public class VokiRatingsDistribution : ValueObject
{
    public override IEnumerable<object> GetEqualityComponents() =>
        [Rating1Count, Rating2Count, Rating3Count, Rating4Count, Rating5Count];

    public VokiRatingsDistribution(
        uint rating1Count, uint rating2Count, uint rating3Count, uint rating4Count, uint rating5Count
    ) {
        Rating1Count = rating1Count;
        Rating2Count = rating2Count;
        Rating3Count = rating3Count;
        Rating4Count = rating4Count;
        Rating5Count = rating5Count;
        TotalCount = Rating1Count + Rating2Count + Rating3Count + Rating4Count + Rating5Count;
        TotalSum = rating1Count * 1 +
                   rating2Count * 2 +
                   rating3Count * 3 +
                   rating4Count * 4 +
                   rating5Count * 5;
    }

    public uint Rating1Count { get; }
    public uint Rating2Count { get; }
    public uint Rating3Count { get; }
    public uint Rating4Count { get; }
    public uint Rating5Count { get; }
    public uint TotalCount { get; }
    public uint TotalSum { get; }

    public static VokiRatingsDistribution Empty => new VokiRatingsDistribution(
        rating1Count: 0,
        rating2Count: 0,
        rating3Count: 0,
        rating4Count: 0,
        rating5Count: 0
    );
}