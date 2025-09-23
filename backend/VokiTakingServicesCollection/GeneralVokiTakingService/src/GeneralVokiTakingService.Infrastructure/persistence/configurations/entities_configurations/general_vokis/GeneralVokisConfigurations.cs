using GeneralVokiTakingService.Domain.general_voki_aggregate;
using InfrastructureShared.Base.persistence.extensions;
using InfrastructureShared.Base.persistence.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiTakingServicesLib.Domain.common;

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
            .Property(x => x.Name)
            .HasConversion<VokiNameConverter>();

        builder
            .HasMany<VokiQuestion>("Questions")
            .WithOne()
            .HasForeignKey("VokiId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.ForceSequentialAnswering);
        builder.Property(x => x.ShuffleQuestions);

        builder
            .HasMany<VokiResult>("_results")
            .WithOne()
            .HasForeignKey("VokiId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property<ImmutableHashSet<VokiTakenRecordId>>("VokiTakenRecordIds")
            .HasGuidBasedIdsImmutableHashSetConversion();

        builder.ComplexProperty(x => x.InteractionSettings, b => {
            b.Property(s => s.AuthenticatedOnlyTaking);
            b.Property(d => d.ResultsVisibility);
            b.Property(d => d.ShowResultsDistribution);
        });
    }
}