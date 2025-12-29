using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters;
using InfrastructureShared.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations.general_vokis;

public class VokiQuestionsConfigurations : IEntityTypeConfiguration<VokiQuestion>
{
    public void Configure(EntityTypeBuilder<VokiQuestion> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder.Property(x => x.Text);
        builder.Property(x => x.ImageSet)
            .HasConversion<VokiQuestionImagesSetConverter>();

        builder.Property(x => x.AnswersType);
        builder.Property(x => x.OrderInVoki);
        builder.Property(x => x.ShuffleAnswers);
        builder
            .Property(x => x.AnswersCountLimit)
            .HasConversion<QuestionAnswersCountLimitConverter>();

        builder
            .HasMany<VokiQuestionAnswer>("Answers")
            .WithOne()
            .HasForeignKey("QuestionId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}