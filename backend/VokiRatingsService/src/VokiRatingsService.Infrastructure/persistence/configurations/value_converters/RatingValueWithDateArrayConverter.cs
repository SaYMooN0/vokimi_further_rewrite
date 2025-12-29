using System.Globalization;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.configurations.value_converters;

internal class RatingValueWithDateArrayConverter : ValueConverter<ImmutableArray<RatingValue>, string[]>
{
    private const char Sep = '|';
    private static readonly CultureInfo C = CultureInfo.InvariantCulture;
    private const string DateFmt = "O"; //ISO 8601

    public RatingValueWithDateArrayConverter() : base(
        ratings => ToStringArray(ratings),
        strings => FromStringArray(strings)
    ) { }

    private static string[] ToStringArray(ImmutableArray<RatingValue> ratings) =>
        ratings.Select(r => $"{r.Value}{Sep}{r.DateTime.ToString(DateFmt, C)}").ToArray();

    private static ImmutableArray<RatingValue> FromStringArray(string[] strs) =>
        strs
            .Select(s => {
                var parts = s.Split(Sep, 2);
                var value = ushort.Parse(parts[0], NumberStyles.None, C);
                var dt = DateTime.ParseExact(parts[1], DateFmt, C, DateTimeStyles.RoundtripKind);
                return RatingValue.Create(value, dt).AsSuccess();
            })
            .ToImmutableArray();
}

internal sealed class RatingValueWithDateArrayComparer : ValueComparer<ImmutableArray<RatingValue>>
{
    public RatingValueWithDateArrayComparer()
        : base(
            (a, b) =>
                a.IsDefault && b.IsDefault ||
                (!a.IsDefault && !b.IsDefault && a.AsEnumerable().SequenceEqual(b.AsEnumerable())),
            a => a.IsDefault ? 0 : a.Aggregate(0, (acc, x) => unchecked(acc * 397 + x.GetHashCode())),
            a => a
        ) { }
}