using SharedKernel;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.app_user_aggregate;
using VokiRatingsService.Domain.voki_aggregate;
using VokiRatingsService.Domain.voki_rating_aggregate.events;

namespace VokiRatingsService.Application.voki_ratings.domain_event_handlers;

internal class NewVokiRatingCreatedHandler : IDomainEventHandler<NewVokiRatingCreatedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IVokiRatingsSnapshotRepository _vokiRatingsSnapshotRepository;

    private readonly IRatingsRepository _ratingsRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public NewVokiRatingCreatedHandler(
        IAppUsersRepository appUsersRepository,
        IDateTimeProvider dateTimeProvider,
        IRatingsRepository ratingsRepository,
        IVokiRatingsSnapshotRepository vokiRatingsSnapshotRepository
    ) {
        _appUsersRepository = appUsersRepository;
        _dateTimeProvider = dateTimeProvider;
        _ratingsRepository = ratingsRepository;
        _vokiRatingsSnapshotRepository = vokiRatingsSnapshotRepository;
    }

    public async Task Handle(NewVokiRatingCreatedEvent e, CancellationToken ct) {
        AppUser user = (await _appUsersRepository.GetById(e.UserId, ct))!;
        user.AddRating(e.RatingId);
        await _appUsersRepository.Update(user, ct);
        
        await VokiRatingsSnapshotUpsertingHelper.UpsertDailySnapshot(
            e.VokiId,
            _dateTimeProvider.UtcNow,
            _vokiRatingsSnapshotRepository,
            _ratingsRepository,
            ct
        );
    }
}