using DraftVokisLib;
using GeneralVokiCreationService.Domain.draft_voki_aggregate;
using GeneralVokiCreationService.Infrastructure.persistence.value_converters;
using InfrastructureShared.persistence.extensions;
using InfrastructureShared.persistence.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiCreationService.Infrastructure.persistence.entities_configurations;

public class DraftVokisConfigurations : IEntityTypeConfiguration<DraftVoki>
{
    public void Configure(EntityTypeBuilder<DraftVoki> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.Name)
            .HasConversion<VokiNameConverter>();

        builder.Ignore(x => x.CoverPath);
        
        builder
            .Property(x => x.PrimaryAuthorId)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property<VokiCoAuthorIdsSet>("CoAuthors")
            .HasConversion<VokiCoAuthorIdsSetConverter>();

    }
}