using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters;

public class VokiQuestionImagesArrayConverter : ValueConverter<ImmutableArray<GeneralVokiQuestionImageKey>, string[]>
{
    public VokiQuestionImagesArrayConverter() : base(
        keys => keys.Select(k => k.ToString()).ToArray(),
        keys => keys.Select(k => new GeneralVokiQuestionImageKey(k)).ToImmutableArray()
    ) { }
}

internal class VokiQuestionImagesArrayComparer : ValueComparer<ImmutableArray<GeneralVokiQuestionImageKey>>
{
    public VokiQuestionImagesArrayComparer() : base(
        (a, b) => a.Equals(b),
        obj => obj.GetHashCode(),
        keys => keys
            .Select(k => new GeneralVokiQuestionImageKey(k.ToString()))
            .ToImmutableArray()
    ) { }
}