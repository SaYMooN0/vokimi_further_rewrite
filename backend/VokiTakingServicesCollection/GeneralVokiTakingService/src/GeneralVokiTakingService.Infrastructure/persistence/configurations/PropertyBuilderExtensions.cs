using GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokimiStorageKeysLib.concrete_keys.general_voki;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<ImmutableArray<GeneralVokiQuestionImageKey>> HasVokiQuestionImagesArrayConverter(
        this PropertyBuilder<ImmutableArray<GeneralVokiQuestionImageKey>> builder
    ) {
        return builder.HasConversion(
            new VokiQuestionImagesArrayConverter(),
            new VokiQuestionImagesArrayComparer()
        );
    }
}