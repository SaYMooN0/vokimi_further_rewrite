using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters;
using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations.general_vokis;

public class VokiQuestionAnswersConfigurations : IEntityTypeConfiguration<VokiQuestionAnswer>
{
    public void Configure(EntityTypeBuilder<VokiQuestionAnswer> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder.Property(x => x.OrderInQuestion);

        builder
            .Property(x => x.TypeData)
            .HasConversion<VokiAnswerTypeDataConverter>();

        builder
            .Property(x => x.RelatedResultIds)
            .HasGuidBasedIdsImmutableHashSetConversion();
    }
}