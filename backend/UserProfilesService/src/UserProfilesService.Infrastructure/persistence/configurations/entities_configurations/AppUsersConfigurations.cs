using InfrastructureShared.persistence.extensions;
using InfrastructureShared.persistence.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfilesService.Domain.app_user_aggregate;
using UserProfilesService.Infrastructure.persistence.configurations.extensions;
using UserProfilesService.Infrastructure.persistence.configurations.value_converters;

namespace UserProfilesService.Infrastructure.persistence.configurations.entities_configurations;

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
            .Property(x => x.UserName)
            .HasConversion<AppUserNameConverter>();

        builder
            .Property(x => x.ProfilePic)
            .HasConversion<AppUserProfilePicKeyConverter>();

        builder.ComplexProperty(x => x.Settings, b => {
            b
                .Property(d => d.AllowCoAuthorInvites)
                .HasColumnName("settings_AllowCoAuthorInvites");
        });

        builder
            .Property(x => x.FavouriteTags)
            .HasTagIdImmutableHashSetHashSetConversion();
    }
}