using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiRatingsService.Domain.voki_ratings_snapshot;
using VokiRatingsService.Infrastructure.persistence.configurations.extensions;

namespace VokiRatingsService.Infrastructure.persistence.configurations.entities_configurations;

internal class VokiRatingsSnapshotConfigurations : IEntityTypeConfiguration<VokiRatingsSnapshot>
{
    public void Configure(EntityTypeBuilder<VokiRatingsSnapshot> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.Values)
            .HasRatingValueWithDateArrayConversion();
    ...
    }
}