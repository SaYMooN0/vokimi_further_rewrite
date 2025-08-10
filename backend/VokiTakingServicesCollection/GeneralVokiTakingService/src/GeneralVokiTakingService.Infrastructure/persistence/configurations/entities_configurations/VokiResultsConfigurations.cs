﻿using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters;
using InfrastructureShared.persistence.extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.entities_configurations;

public class VokiResultsConfigurations : IEntityTypeConfiguration<VokiResult>
{
    public void Configure(EntityTypeBuilder<VokiResult> builder) {
        builder
            .HasKey(x => x.Id);
        builder
            .Property(x => x.Id)
            .ValueGeneratedNever()
            .HasGuidBasedIdConversion();

        builder.Property(x => x.Name);
        builder.Property(x => x.Text);

        builder
            .Property(x => x.Image)
            .HasConversion<VokiResultImageConverter>();
    }
}