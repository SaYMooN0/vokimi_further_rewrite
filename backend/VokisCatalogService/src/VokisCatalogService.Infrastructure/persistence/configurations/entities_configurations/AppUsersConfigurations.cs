using System.Collections.Immutable;
using InfrastructureShared.Base.persistence.extensions;
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
            .Property(x => x.InitializedVokiIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder
            .Property(x => x.CoAuthoredVokiIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder
            .HasOne(x => x.TakenVokis)
            .WithOne()
            .HasForeignKey<AppUserId>("UserId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}