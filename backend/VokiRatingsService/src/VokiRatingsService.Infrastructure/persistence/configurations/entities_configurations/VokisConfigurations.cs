using InfrastructureShared.Base.persistence.extensions;
using InfrastructureShared.Base.persistence.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.common.vokis;
using VokiRatingsService.Domain.voki_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.configurations.entities_configurations;

internal class VokisConfigurations : IEntityTypeConfiguration<Voki>
{
    public void Configure(EntityTypeBuilder<Voki> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.RatingIds)
            .HasGuidBasedIdsImmutableHashSetConversion();
        
        builder
            .Property<VokiManagersIdsSet>("ManagersSet")
            .HasConversion<VokiManagersIdsSetConverter>();
    }
}