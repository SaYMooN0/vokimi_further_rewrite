using System.Text.Json;
using InfrastructureShared.persistence.value_converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfrastructureShared.persistence.extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TId> HasGuidBasedIdConversion<TId>(
        this PropertyBuilder<TId> builder
    ) where TId : GuidBasedId {
        return builder.HasConversion(new GuidBasedIdConverter<TId>());
    }

    public static PropertyBuilder<HashSet<T>> HasGuidBasedIdsHashSetConversion<T>(
        this PropertyBuilder<HashSet<T>> builder
    ) where T : GuidBasedId {
        return builder.HasConversion(
            new GuidBasedIdHashSetConverter<T>(),
            new GuidBasedIdHashSetComparer<T>()
        );
    }
}