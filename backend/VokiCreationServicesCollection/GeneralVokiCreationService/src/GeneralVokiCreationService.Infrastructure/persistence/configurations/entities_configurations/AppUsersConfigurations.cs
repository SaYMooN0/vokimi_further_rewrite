using System.Collections.Immutable;
using GeneralVokiCreationService.Domain.app_user_aggregate;
using InfrastructureShared.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.entities_configurations;

internal class AppUsersConfigurations : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property<ImmutableHashSet<VokiId>>("InitializedVokiIds")
            .HasGuidBasedIdsImmutableHashSetConversion();
        
        builder
            .Property<ImmutableHashSet<VokiId>>("CoAuthoredVokiIds")
            .HasGuidBasedIdsImmutableHashSetConversion();
    }
}