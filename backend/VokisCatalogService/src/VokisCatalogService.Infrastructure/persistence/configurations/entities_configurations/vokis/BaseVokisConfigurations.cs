using InfrastructureShared.persistence.extensions;
using InfrastructureShared.persistence.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokimiStorageKeysLib.voki_cover;
using VokisCatalogService.Domain.voki_aggregate;
using VokisCatalogService.Infrastructure.persistence.configurations.extensions;
using VokisCatalogService.Infrastructure.persistence.configurations.value_converters;

namespace VokisCatalogService.Infrastructure.persistence.configurations.entities_configurations.vokis;

public class BaseVokisConfigurations : IEntityTypeConfiguration<BaseVoki>
{
    public void Configure(EntityTypeBuilder<BaseVoki> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.Name)
            .HasConversion<VokiNameConverter>();

        builder
            .Property(x => x.PublicationDate);
        builder
            .Property(x => x.Cover)
            .HasConversion<VokiCoverKeyConverter>();

        builder
            .Property(x => x.PrimaryAuthorId)
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.CoAuthorIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder.OwnsOne(v => v.Details, d => {
            d.Property(p => p.Language).HasColumnName("Details_Language");
            d.Property(p => p.IsAgeRestricted).HasColumnName("Details_IsAgeRestricted");
            d.Property(p => p.Description).HasColumnName("Details_Description");
        });


        builder
            .Property(x => x.Tags)
            .HasTagIdImmutableHashSetHashSetConversion();

        builder.Property(x => x.PublicationDate);

        builder.Property(x => x.RatingsCount);
        builder.Property(x => x.CommentsCount);
        builder.Property(x => x.VokiTakingsCount);
    }
}