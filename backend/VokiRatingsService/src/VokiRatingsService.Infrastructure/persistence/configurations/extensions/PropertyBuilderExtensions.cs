using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiRatingsService.Domain.voki_rating_aggregate;
using VokiRatingsService.Infrastructure.persistence.configurations.value_converters;

namespace VokiRatingsService.Infrastructure.persistence.configurations.extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<ImmutableArray<RatingValueWithDate>> HasRatingValueWithDateArrayConversion(
        this PropertyBuilder<ImmutableArray<RatingValueWithDate>> builder
    ) {
        return builder.HasConversion(
            new RatingValueWithDateArrayConverter(),
            new RatingValueWithDateArrayComparer()
        );
    }
}