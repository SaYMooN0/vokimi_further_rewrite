
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserProfilesService.Infrastructure.persistence.configurations.value_converters;

namespace UserProfilesService.Infrastructure.persistence.configurations.extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<ImmutableHashSet<VokiTagId>> HasTagIdImmutableHashSetConversion(
        this PropertyBuilder<ImmutableHashSet<VokiTagId>> builder
    ) {
        return builder.HasConversion(
            new TagIdImmutableHashSetConverter(),
            new TagIdImmutableHashSetComparer()
        );
    }
    public static PropertyBuilder<HashSet<Language>> HasLanguagesImmutableHashSetConversion(
        this PropertyBuilder<HashSet<Language>> builder
    ) {
        return builder.HasConversion(
            new LanguagesHashSetConverter(),
            new LanguagesHashSetComparer()
        ).HasColumnType("text[]");
    }
}