using System.Collections.Immutable;
using InfrastructureShared.Base.persistence.value_converters.guid_based_ids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfrastructureShared.Base.persistence.extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TId> HasGuidBasedIdConversion<TId>(
        this PropertyBuilder<TId> builder
    ) where TId : notnull, GuidBasedId {
        return builder.HasConversion(new GuidBasedIdConverter<TId>());
    }


    public static PropertyBuilder<TId?> HasNullableGuidBasedIdConversion<TId>(
        this PropertyBuilder<TId?> builder
    ) where TId : GuidBasedId {
        return builder.HasConversion(new NullableGuidBasedIdConverter<TId>());
    }

    public static PropertyBuilder<HashSet<T>> HasGuidBasedIdsHashSetConversion<T>(
        this PropertyBuilder<HashSet<T>> builder
    ) where T : GuidBasedId {
        return builder.HasConversion(
            new GuidBasedIdHashSetConverter<T>(),
            new GuidBasedIdHashSetComparer<T>()
        );
    }

    public static PropertyBuilder<ImmutableHashSet<T>> HasGuidBasedIdsImmutableHashSetConversion<T>(
        this PropertyBuilder<ImmutableHashSet<T>> builder
    ) where T : GuidBasedId {
        return builder.HasConversion(
            new GuidBasedIdImmutableHashSetConverter<T>(),
            new GuidBasedIdImmutableHashSetComparer<T>()
        );
    }
}