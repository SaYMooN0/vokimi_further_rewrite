using InfrastructureShared.Base.persistence.extensions;
using InfrastructureShared.Base.persistence.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.common.app_users;
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
            .Property(x => x.UniqueName)
            .HasConversion<UserUniqueNameConverter>()
            .HasColumnName("UniqueName")
            .HasColumnType($"varchar({UserUniqueName.MaxLength + 10})")
            .IsRequired();

        builder
            .Property(x => x.DisplayName)
            .HasConversion<UserDisplayNameConverter>()
            .HasColumnName("DisplayName")
            .HasColumnType($"varchar({UserDisplayName.MaxLength + 10})")
            .IsRequired();

        builder.Property<string>("SearchableName")
            .HasColumnName("searchable_name")
            .HasColumnType("text")
            .HasComputedColumnSql("lower(\"UniqueName\" || ' ' || \"DisplayName\")", stored: true)
            .ValueGeneratedOnAddOrUpdate()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);


        builder
            .Property(x => x.ProfilePic)
            .HasConversion<AppUserProfilePicKeyConverter>();

        builder
            .Property(x => x.FavoriteTags)
            .HasTagIdImmutableHashSetConversion();

        builder
            .Property(x => x.PreferredLanguages)
            .HasLanguagesImmutableHashSetConversion();

        builder.ComplexProperty(x => x.Settings, b => {
            b
                .Property(d => d.AllowCoAuthorInvites)
                .HasColumnName("settings_AllowCoAuthorInvites");
        });
        
       
    }
}