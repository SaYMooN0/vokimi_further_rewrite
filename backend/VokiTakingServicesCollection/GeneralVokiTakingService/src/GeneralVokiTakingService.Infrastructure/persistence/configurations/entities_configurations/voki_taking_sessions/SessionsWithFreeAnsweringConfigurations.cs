using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using GeneralVokiTakingService.Infrastructure.persistence.configurations.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations.voki_taking_sessions;

public class SessionsWithFreeAnsweringConfigurations : IEntityTypeConfiguration<SessionWithFreeAnswering>
{
    public void Configure(EntityTypeBuilder<SessionWithFreeAnswering> builder) {
        builder
            .Property<ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>>(
                "_questionsWithSavedAnswers")
            .HasFreeTakingSavedQuestionsConversion()
            .HasColumnName("QuestionsWithSavedAnswers");
    }
}