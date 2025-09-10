using AlbumsService.Domain.voki_album_aggregate;
using InfrastructureShared.Base.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlbumsService.Infrastructure.persistence.configurations.entities_configurations;

internal class VokiAlbumsConfiguration : IEntityTypeConfiguration<VokiAlbum>
{
    public void Configure(EntityTypeBuilder<VokiAlbum> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();
    }
}