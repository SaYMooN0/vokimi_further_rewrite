using MassTransit;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.integration_events.voki_publishing;
using VokimiStorageKeysLib.concrete_keys;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;
using VokisCatalogService.Domain.voki_aggregate.voki_types;

namespace VokisCatalogService.Application.vokis.integration_event_handlers;

public class GeneralVokiPublishedIntegrationEventHandler : IConsumer<GeneralVokiPublishedIntegrationEvent>
{
    private readonly IGeneralVokisRepository _generalVokisRepository;

    public GeneralVokiPublishedIntegrationEventHandler(IGeneralVokisRepository generalVokisRepository) {
        _generalVokisRepository = generalVokisRepository;
    }

    public async Task Consume(ConsumeContext<GeneralVokiPublishedIntegrationEvent> context) {
        var e = context.Message;
        bool anyAudios = CheckIfVokiHasAnyAudios(e);

        GeneralVoki voki = GeneralVoki.CreateNew(
            e.VokiId,
            new VokiName(e.Name),
            new VokiCoverKey(e.Cover),
            e.PrimaryAuthorId,
            e.CoAuthors.ToImmutableHashSet(),
            VokiManagersIdsSet.Create(e.Managers.ToImmutableHashSet()).AsSuccess(),
            new VokiDetails(e.Description, e.HasMatureContent, e.Language),
            tags: e.Tags.ToImmutableHashSet(),
            e.PublishingDate,
            GeneralVokiInteractionSettings.Create(
                signedInOnlyTaking: e.InteractionSettings.SignedInOnlyTaking,
                resultsVisibility: e.InteractionSettings.ResultsVisibility,
                showResultsDistribution: e.InteractionSettings.ShowResultsDistribution
            ).AsSuccess(),
            questionsCount: (ushort)e.Questions.Length,
            resultsCount: (ushort)e.Results.Length,
            anyAudios: anyAudios,
            forceSequentialAnswering: e.ForceSequentialAnswering,
            shuffleQuestions: e.ShuffleQuestions
        );
        await _generalVokisRepository.Add(voki, context.CancellationToken);
    }

    private static bool CheckIfVokiHasAnyAudios(GeneralVokiPublishedIntegrationEvent v) {
        bool anyAudioAnswers = v.Questions.Any(q => q.AnswersType.HasAudio());
        return anyAudioAnswers;
    }
}