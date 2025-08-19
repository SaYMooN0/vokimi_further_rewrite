using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfilesService.Infrastructure.persistence.configurations.value_converters;

namespace UserProfilesService.Infrastructure.persistence.configurations.extensions;

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