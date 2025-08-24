using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters;
using InfrastructureShared.Base.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.entities_configurations;


public class VokiQuestionAnswersConfigurations : IEntityTypeConfiguration<VokiQuestionAnswer>
{
    public void Configure(EntityTypeBuilder<VokiQuestionAnswer> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.TypeData)
            .HasConversion<VokiAnswerTypeDataConverter>();

        builder
            .Property(x => x.RelatedResultIds)
            .HasGuidBasedIdsImmutableHashSetConversion();
    }
}