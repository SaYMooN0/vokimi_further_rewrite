using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokisCatalogService.Domain.voki_aggregate;
using VokisCatalogService.Domain.voki_aggregate.voki_types;

namespace VokisCatalogService.Infrastructure.persistence.configurations.entities_configurations.vokis;

public class TierListVokisConfigurations : IEntityTypeConfiguration<TierListVoki>
{
    public void Configure(EntityTypeBuilder<TierListVoki> builder) {
        builder.ToTable("VokisTierList");

        builder.HasInteractionSettingsAsComplexProperty(x => x.InteractionSettings);
    }
}