using MassTransit;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.integration_events.voki_publishing;
using VokimiStorageKeysLib.concrete_keys;
using VokisCatalogService.Domain.common.interfaces.repositories;
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
        bool anyAudioAnswers = e.Questions.Any(q => q.AnswersType.HasAudio());

        GeneralVoki voki = GeneralVoki.CreateNew(
            e.VokiId,
            new VokiName(e.Name),
            new VokiCoverKey(e.Cover),
            e.PrimaryAuthorId,
            e.CoAuthors.ToImmutableHashSet(),
            new VokiDetails(e.Description, e.HasMatureContent, e.Language),
            tags: e.Tags.ToImmutableHashSet(),
            e.PublishingDate,
            questionsCount: (ushort)e.Questions.Length,
            resultsCount: (ushort)e.Results.Length,
            anyAudioAnswers: anyAudioAnswers
        );
        await _generalVokisRepository.Add(voki);
    }
}