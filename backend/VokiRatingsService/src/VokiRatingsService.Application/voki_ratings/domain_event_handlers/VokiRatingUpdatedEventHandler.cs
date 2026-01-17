using SharedKernel;
using SharedKernel.domain;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_rating_aggregate.events;

namespace VokiRatingsService.Application.voki_ratings.domain_event_handlers;

internal class VokiRatingUpdatedHandler : IDomainEventHandler<VokiRatingUpdatedEvent>
{
    private readonly IVokiRatingsSnapshotRepository _vokiRatingsSnapshotRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IRatingsRepository _ratingsRepository;

    public VokiRatingUpdatedHandler(
        IVokiRatingsSnapshotRepository vokiRatingsSnapshotRepository,
        IDateTimeProvider dateTimeProvider,
        IRatingsRepository ratingsRepository
    ) {
        _vokiRatingsSnapshotRepository = vokiRatingsSnapshotRepository;
        _dateTimeProvider = dateTimeProvider;
        _ratingsRepository = ratingsRepository;
    }

    public Task Handle(VokiRatingUpdatedEvent e, CancellationToken ct) {
        throw new();
        // await VokiRatingsSnapshotUpsertingHelper.UpsertDailySnapshotOnRatingUpdated(
        //     e.VokiId,
        //     _dateTimeProvider.UtcNow,
        //     _vokiRatingsSnapshotRepository,
        //     _ratingsRepository,
        //     ct
        // );
    }
}