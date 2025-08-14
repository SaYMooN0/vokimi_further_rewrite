using MassTransit;
using SharedKernel.common.vokis;
using SharedKernel.integration_events.voki_publishing;
using VokisCatalogService.Domain.common.interfaces.repositories;
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
            e.PrimaryAuthorId,
            e.CoAuthors.ToImmutableHashSet(),
            tags: e.Tags.ToImmutableHashSet(),
            questionsCount: (ushort)e.Questions.Length,
            resultsCount: (ushort)e.Results.Length,
            anyAudioAnswers: anyAudioAnswers
        );
        await _generalVokisRepository.Add(voki);
    }
}