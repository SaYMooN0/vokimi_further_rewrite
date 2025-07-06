using CoreVokiCreationService.Domain.draft_voki_aggregate;
using InfrastructureShared.persistence.extensions;
using InfrastructureShared.persistence.value_converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreVokiCreationService.Infrastructure.persistence.entities_configurations;

public class DraftVokisConfigurations : IEntityTypeConfiguration<DraftVoki>
{
    public void Configure(EntityTypeBuilder<DraftVoki> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion()
            .HasColumnName("Id");

        builder
            .Property(x => x.Name)
            .HasConversion<VokiNameConverter>();

        builder.Ignore(x => x.CoverPath);
        
        builder
            .Property(x => x.PrimaryAuthorId)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion()
            .HasColumnName("PrimaryAuthorId");

        builder
            .Property(x => x.CoAuthorsIds)
            .HasGuidBasedIdsImmutableHashSetConversion()
            .HasColumnName("CoAuthorsIds");

        builder
            .Property(x => x.InvitedForCoAuthorUserIds)
            .HasGuidBasedIdsImmutableHashSetConversion();
        
        builder
            .Property(x => x.CreationDate)
            .HasColumnName("CreationDate");
    }
}