using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_rating_aggregate;
using VokiRatingsService.Infrastructure.persistence.configurations.value_converters;

namespace VokiRatingsService.Infrastructure.persistence.configurations.extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<ImmutableArray<RatingValue>> HasRatingValueWithDateArrayConversion(
        this PropertyBuilder<ImmutableArray<RatingValue>> builder
    ) {
        return builder.HasConversion(
            new RatingValueWithDateArrayConverter(),
            new RatingValueWithDateArrayComparer()
        );
    }
}