using GeneralVokiTakingService.Domain.general_voki_aggregate;
using InfrastructureShared.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations;

public class GeneralVokisConfigurations : IEntityTypeConfiguration<GeneralVoki>
{
    public void Configure(EntityTypeBuilder<GeneralVoki> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .HasMany<VokiQuestion>("Questions")
            .WithOne()
            .HasForeignKey("VokiId");
        
        builder.Property<bool>("ShuffleQuestions");
        builder.Property(x => x.ForceSequentialAnswering);
        
        builder
            .HasMany<VokiResult>("Results")
            .WithOne()
            .HasForeignKey("VokiId");

        builder
            .Property(x => x.VokiTakenRecordIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

       
    }
}