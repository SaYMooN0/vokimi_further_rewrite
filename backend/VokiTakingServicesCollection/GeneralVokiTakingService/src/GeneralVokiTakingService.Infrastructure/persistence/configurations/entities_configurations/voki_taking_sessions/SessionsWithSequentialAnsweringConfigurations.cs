using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.sequential_answering;
using GeneralVokiTakingService.Infrastructure.persistence.configurations.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations.voki_taking_sessions;

public class SessionsWithSequentialAnsweringConfigurations : IEntityTypeConfiguration<SessionWithSequentialAnswering>
{
    public void Configure(EntityTypeBuilder<SessionWithSequentialAnswering> builder) {
        builder.ToTable("SessionsWithSequentialAnswering");
        builder.HasBaseType<BaseVokiTakingSession>();

        builder.Property(x => x.CurrentQuestionOrder);
        builder
            .Property<ImmutableArray<SequentialTakingAnsweredQuestion>>(" _answered")
            .HasSequentialTakingAnsweredQuestionsConversion()
            .HasColumnName("AnsweredQuestions");
    }
}