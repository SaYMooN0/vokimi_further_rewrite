using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using InfrastructureShared.Base.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations.voki_taking_sessions;

public class BaseVokiTakingSessionsConfigurations : IEntityTypeConfiguration<BaseVokiTakingSession>
{
    public void Configure(EntityTypeBuilder<BaseVokiTakingSession> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();
    }
}