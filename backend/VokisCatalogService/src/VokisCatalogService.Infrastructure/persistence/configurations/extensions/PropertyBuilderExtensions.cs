using System.Collections.Immutable;
using InfrastructureShared.persistence.value_converters.guid_based_ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokisCatalogService.Infrastructure.persistence.configurations.value_converters;

namespace VokisCatalogService.Infrastructure.persistence.configurations.extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<ImmutableHashSet<VokiTagId>> HasTagIdImmutableHashSetHashSetConversion(
        this PropertyBuilder<ImmutableHashSet<VokiTagId>> builder
    ) {
        return builder.HasConversion(
            new TagIdImmutableHashSetHashSetConverter(),
            new TagIdImmutableHashSetHashSetComparer()
        );
    }
}