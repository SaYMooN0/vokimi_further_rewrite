using CoreVokiCreationService.Domain.draft_voki_aggregate;
using InfrastructureShared.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreVokiCreationService.Infrastructure.persistence.entities_configurations;

public class DraftVokisConfigurations: IEntityTypeConfiguration<DraftVoki>
{
    public void Configure(EntityTypeBuilder<DraftVoki> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

    }
}