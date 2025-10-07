using InfrastructureShared.Base.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokiRatingsService.Domain.voki_rating_aggregate;
using VokiRatingsService.Infrastructure.persistence.configurations.extensions;

namespace VokiRatingsService.Infrastructure.persistence.configurations.entities_configurations;

internal class VokiRatingsConfigurations : IEntityTypeConfiguration<VokiRating>
{
    public void Configure(EntityTypeBuilder<VokiRating> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.UserId)
            .HasGuidBasedIdConversion();

        builder
            .Property(x => x.VokiId)
            .HasGuidBasedIdConversion();

        builder.ComplexProperty(x => x.Current, cp => {
            cp
                .Property(x => x.Value)
                .HasColumnName("Current_Value");
            cp
                .Property(x => x.DateTime)
                .HasColumnName("Current_DateTime");
        });
        builder
            .HasOne(x => x.History)
            .WithOne()
            .HasForeignKey<RatingHistory>("RatingId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}