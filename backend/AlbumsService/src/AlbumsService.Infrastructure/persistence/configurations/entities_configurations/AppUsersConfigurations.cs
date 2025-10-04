using AlbumsService.Domain.app_user_aggregate;
using InfrastructureShared.Base.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlbumsService.Infrastructure.persistence.configurations.entities_configurations;

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
            .Property(x => x.AlbumIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder.ComplexProperty(x => x.AutoAlbumsAppearance, a => {
            a.Property(d => d.TakenMainColor);
            a.Property(d => d.TakenSecondaryColor);
            a.Property(d => d.RatedMainColor);
            a.Property(d => d.RatedSecondaryColor);
            a.Property(d => d.CommentedMainColor);
            a.Property(d => d.CommentedSecondaryColor);
        });
    }
}