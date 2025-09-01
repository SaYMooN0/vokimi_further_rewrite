using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations.voki_taking_sessions;

public class SessionsWithFreeAnsweringConfigurations : IEntityTypeConfiguration<SessionWithFreeAnswering>
{
    public void Configure(EntityTypeBuilder<SessionWithFreeAnswering> builder) {
        builder.ToTable("SessionsWithFreeAnswering");
        builder.HasBaseType<BaseVokiTakingSession>();

        builder
            .Ignore(x => x.AnsweredQuestions);
    }
}