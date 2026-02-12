using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokisCatalogService.Domain.voki_aggregate;
using VokisCatalogService.Domain.voki_aggregate.voki_types;

namespace VokisCatalogService.Infrastructure.persistence.configurations.entities_configurations.vokis;

public class GeneralVokisConfigurations : IEntityTypeConfiguration<GeneralVoki>
{
    public void Configure(EntityTypeBuilder<GeneralVoki> builder) {
        builder.ToTable("VokisGeneral");

        builder.HasInteractionSettingsAsComplexProperty(x => x.InteractionSettings);
        builder.Property(x => x.QuestionsCount);
        builder.Property(x => x.ResultsCount);
        builder.Property(x => x.AnyAudios);
        builder.Property(x => x.ForceSequentialAnswering);
        builder.Property(x => x.ShuffleQuestions);
    }
}