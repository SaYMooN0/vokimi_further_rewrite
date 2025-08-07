using InfrastructureShared.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Infrastructure.persistence.configurations.entities_configurations;

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
            .Property(x => x.PrimaryAuthorId)
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.CoAuthorIds)
            .HasGuidBasedIdsImmutableHashSetConversion();
    }
}