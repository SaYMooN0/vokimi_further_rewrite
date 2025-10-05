using InfrastructureShared.Base.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiRatingsService.Domain.voki_rating_aggregate;
using VokiRatingsService.Infrastructure.persistence.configurations.extensions;

namespace VokiRatingsService.Infrastructure.persistence.configurations.entities_configurations;

internal class RatingHistoryConfigurations : IEntityTypeConfiguration<RatingHistory>
{
    public void Configure(EntityTypeBuilder<RatingHistory> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.Values)
            .HasRatingValueWithDateArrayConversion();
    }
}