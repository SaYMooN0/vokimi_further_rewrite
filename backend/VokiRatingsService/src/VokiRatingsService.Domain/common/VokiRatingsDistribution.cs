using SharedKernel.exceptions;

namespace VokiRatingsService.Domain.common;

public class VokiRatingsDistribution : ValueObject
{
    public override IEnumerable<object> GetEqualityComponents() =>
        [Rating1Count, Rating2Count, Rating3Count, Rating4Count, Rating5Count];

    public VokiRatingsDistribution(
        uint rating1Count, uint rating2Count, uint rating3Count, uint rating4Count, uint rating5Count
    )
    {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(
            rating1Count, rating2Count, rating3Count, rating4Count, rating5Count
        ));
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

    public static ErrOrNothing CheckForErr(
        uint rating1Count, uint rating2Count, uint rating3Count, uint rating4Count, uint rating5Count
    )
    {
        return ErrOrNothing.Nothing;
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

    public static VokiRatingsDistribution FromRating(RatingValue rating) => rating.Value switch
    {
        1 => new VokiRatingsDistribution(rating1Count: 1, 0, 0, 0, 0),
        2 => new VokiRatingsDistribution(rating1Count: 0, 1, 0, 0, 0),
        3 => new VokiRatingsDistribution(rating1Count: 0, 0, 1, 0, 0),
        4 => new VokiRatingsDistribution(rating1Count: 0, 0, 0, 1, 0),
        5 => new VokiRatingsDistribution(rating1Count: 0, 0, 0, 0, 1),
        _ => throw new ArgumentException($"Invalid rating value: {rating.Value}")
    };

    public static VokiRatingsDistribution FromRatingsArray(RatingValue[] ratings)
    {
        if (ratings.Length == 0)
        {
            return Empty;
        }

        uint r1 = 0, r2 = 0, r3 = 0, r4 = 0, r5 = 0;

        foreach (var rating in ratings)
        {
            switch (rating.Value)
            {
                case 1:
                    r1++;
                    break;
                case 2:
                    r2++;
                    break;
                case 3:
                    r3++;
                    break;
                case 4:
                    r4++;
                    break;
                case 5:
                    r5++;
                    break;
            }
        }

        return new VokiRatingsDistribution(
            rating1Count: r1,
            rating2Count: r2,
            rating3Count: r3,
            rating4Count: r4,
            rating5Count: r5
        );
    }

    public VokiRatingsDistribution WithNewRating(RatingValue rating) => rating.Value switch
    {
        1 => new VokiRatingsDistribution(Rating1Count + 1, Rating2Count, Rating3Count, Rating4Count, Rating5Count),
        2 => new VokiRatingsDistribution(Rating1Count, Rating2Count + 1, Rating3Count, Rating4Count, Rating5Count),
        3 => new VokiRatingsDistribution(Rating1Count, Rating2Count, Rating3Count + 1, Rating4Count, Rating5Count),
        4 => new VokiRatingsDistribution(Rating1Count, Rating2Count, Rating3Count, Rating4Count + 1, Rating5Count),
        5 => new VokiRatingsDistribution(Rating1Count, Rating2Count, Rating3Count, Rating4Count, Rating5Count + 1),
        _ => throw new ArgumentException($"Invalid rating value: {rating.Value}")
    };

    private Dictionary<ushort, uint> ToDictionary() => new()
    {
        [1] = Rating1Count,
        [2] = Rating2Count,
        [3] = Rating3Count,
        [4] = Rating4Count,
        [5] = Rating5Count,
    };

    public ErrOr<VokiRatingsDistribution> WithOneUpdatedRating(RatingValue oldRating, RatingValue newRating)
    {
        if (oldRating.Value == newRating.Value)
        {
            return this;
        }

        var d = ToDictionary();
        if (d[oldRating.Value] == 0)
        {
            return ErrFactory.Conflict($"Attempted to decrement rating {oldRating.Value} count below zero");
        }

        d[oldRating.Value] -= 1;
        d[newRating.Value] += 1;
        return new VokiRatingsDistribution(d[1], d[2], d[3], d[4], d[5]);
    }
}