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
    // public static PropertyBuilder<ImmutableHashSet<T>> HasGuidBasedIdsHashSetConversion<T>(
    //     this PropertyBuilder<ImmutableHashSet<T>> builder
    // ) where T : GuidBasedId {
    //     return builder.HasConversion(
    //         new GuidBasedIdImmutableHashSetHashSetConverter<T>(),
    //         new GuidBasedIdImmutableHashSetHashSetComparer<T>()
    //     );
    // }
}