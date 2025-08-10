using System.Collections.Immutable;
using InfrastructureShared.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokisCatalogService.Domain.app_user_aggregate;

namespace VokisCatalogService.Infrastructure.persistence.configurations.entities_configurations;

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
            .Property<ImmutableHashSet<VokiId>>(x=>x.InitializedVokiIds)
            .HasGuidBasedIdsImmutableHashSetConversion();
        
        builder
            .Property<ImmutableHashSet<VokiId>>("CoAuthoredVokiIds")
            .HasGuidBasedIdsImmutableHashSetConversion();
    }
}