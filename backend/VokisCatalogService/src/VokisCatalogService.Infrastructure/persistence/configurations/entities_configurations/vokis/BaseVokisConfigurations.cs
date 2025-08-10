using InfrastructureShared.persistence.extensions;
using InfrastructureShared.persistence.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokisCatalogService.Domain.voki_aggregate;
using VokisCatalogService.Infrastructure.persistence.configurations.extensions;

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
            .Property(x => x.PrimaryAuthorId)
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.CoAuthorIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder
            .Property(x => x.Tags)
            .HasTagIdImmutableHashSetHashSetConversion();

        builder.Property(x => x.LikesCount);
        builder.Property(x => x.CommentsCount);
        builder.Property(x => x.VokiTakingsCount);
    }
}