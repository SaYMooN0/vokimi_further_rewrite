using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using InfrastructureShared.Base.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations;

public class GeneralVokiTakenRecordsConfigurations : IEntityTypeConfiguration<GeneralVokiTakenRecord>
{
    public void Configure(EntityTypeBuilder<GeneralVokiTakenRecord> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();
        
        builder
            .Property(x => x.TakenVokiId)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();
        
    }
}