using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokisCatalogService.Domain.voki_aggregate;
using VokisCatalogService.Domain.voki_aggregate.voki_types;

namespace VokisCatalogService.Infrastructure.persistence.configurations.entities_configurations.vokis;

public class ScoringVokisConfigurations : IEntityTypeConfiguration<ScoringVoki>
{
    public void Configure(EntityTypeBuilder<ScoringVoki> builder) {
        builder.ToTable("VokisScoring");
        builder.HasBaseType<BaseVoki>();
        builder.HasInteractionSettingsAsComplexProperty(x => x.InteractionSettings);
    }
}