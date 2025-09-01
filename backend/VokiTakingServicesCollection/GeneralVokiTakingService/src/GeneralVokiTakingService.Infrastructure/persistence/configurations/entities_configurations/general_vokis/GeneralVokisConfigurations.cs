using GeneralVokiTakingService.Domain.general_voki_aggregate;
using InfrastructureShared.Base.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations.general_vokis;

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
            .HasForeignKey("VokiId")
            .IsRequired()            
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(x => x.ForceSequentialAnswering);
        builder.Property(x => x.ShuffleQuestions);
        
        builder
            .HasMany<VokiResult>("Results")
            .WithOne()
            .HasForeignKey("VokiId")
            .IsRequired()            
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.VokiTakenRecordIds)
            .HasGuidBasedIdsImmutableHashSetConversion();

       
    }
}