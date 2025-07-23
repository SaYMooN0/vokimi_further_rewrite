using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using GeneralVokiCreationService.Infrastructure.persistence.configurations.value_converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiCreationService.Infrastructure.persistence.configurations;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<VokiQuestionImagesSet> HasVokiQuestionImagesSetConversion(
        this PropertyBuilder<VokiQuestionImagesSet> builder
    ) {
        return builder.HasConversion(
            new VokiQuestionImagesSetConverter(),
            new VokiQuestionImagesSetComparer()
        );
    }
}