using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters;
using InfrastructureShared.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.entities_configurations;

public class VokiQuestionsConfigurations : IEntityTypeConfiguration<VokiQuestion>
{
    public void Configure(EntityTypeBuilder<VokiQuestion> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.Text)
            .HasConversion<VokiQuestionTextConverter>();

        builder
            .Property(x => x.Images)
            .HasConversion<VokiQuestionImagesSetConverter>();

        builder.Ignore(x => x.Answers);
        builder
            .HasMany<VokiQuestionAnswer>("_answers")
            .WithOne()
            .HasForeignKey("QuestionId");

        builder
            .Property(x => x.AnswersCountLimit)
            .HasConversion<QuestionAnswersCountLimitConverter>();
    }
}