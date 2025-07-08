﻿using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.domain.ids;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Infrastructure.persistence.value_converters;

public class VokiTagsSetConverter : ValueConverter<VokiTagsSet, string[]>
{
    public VokiTagsSetConverter()
        : base(
            set => set.Tags.Select(id => id.ToString()).ToArray(),
            tags => VokiTagsSet.Create(
                tags.Select(t => new VokiTagId(t)).ToImmutableHashSet()
            ).AsSuccess()
        ) { }
}

internal class VokiTagsSetComparer : ValueComparer<VokiTagsSet>
{
    public VokiTagsSetComparer() : base(
        (set1, set2) => set1.Tags.SetEquals(set2.Tags),
        set => set.Tags.Aggregate(0, (hash, tagId) => HashCode.Combine(hash, tagId.GetHashCode())),
        set => VokiTagsSet.Create(set.Tags.ToImmutableHashSet()).AsSuccess()
    )
    { }
}