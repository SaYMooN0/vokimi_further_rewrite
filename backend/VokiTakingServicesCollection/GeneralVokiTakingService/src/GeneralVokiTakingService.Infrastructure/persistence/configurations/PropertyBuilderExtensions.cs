using GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VokimiStorageKeysLib.general_voki.question_image;

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