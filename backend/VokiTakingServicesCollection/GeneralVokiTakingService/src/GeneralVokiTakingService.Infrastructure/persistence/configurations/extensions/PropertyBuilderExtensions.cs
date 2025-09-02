using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate.sequential_answering;
using GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<ImmutableArray<SequentialTakingAnsweredQuestion>>
        HasSequentialTakingAnsweredQuestionsConversion(
            this PropertyBuilder<ImmutableArray<SequentialTakingAnsweredQuestion>> builder
        ) {
        return builder.HasConversion(
            new SequentialTakingAnsweredQuestionsArrayConverter(),
            new SequentialTakingAnsweredQuestionsArrayComparer()
        );
    }

    public static PropertyBuilder<ImmutableArray<TakingSessionExpectedQuestion>> HasSessionExpectedQuestionConversion(
        this PropertyBuilder<ImmutableArray<TakingSessionExpectedQuestion>> builder
    ) {
        return builder.HasConversion(
            new TakingSessionExpectedQuestionsArrayConverter(),
            new TakingSessionExpectedQuestionsArrayComparer()
        );
    }
}