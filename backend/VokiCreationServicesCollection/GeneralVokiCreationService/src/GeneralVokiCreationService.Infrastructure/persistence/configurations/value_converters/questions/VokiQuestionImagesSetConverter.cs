using System.Collections.Immutable;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.general_voki.question_image;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters.questions;

public class VokiQuestionImagesSetConverter : ValueConverter<VokiQuestionImagesSet, string[]>
{
    public VokiQuestionImagesSetConverter() : base(
        set => set.Keys.Select(k => k.ToString()).ToArray(),
        keys => VokiQuestionImagesSet.Create(
            keys.Select(k => new GeneralVokiQuestionImageKey(k)).ToImmutableArray()
        ).AsSuccess()
    ) { }
}

internal class VokiQuestionImagesSetComparer : ValueComparer<VokiQuestionImagesSet>
{
    public VokiQuestionImagesSetComparer() : base(
        (a, b) => a.Equals(b),
        obj => obj.GetHashCode(),
        obj => VokiQuestionImagesSet.Create(
            obj.Keys.Select(k => new GeneralVokiQuestionImageKey(k.ToString()))
                .ToImmutableArray()
        ).AsSuccess()
    ) { }
}