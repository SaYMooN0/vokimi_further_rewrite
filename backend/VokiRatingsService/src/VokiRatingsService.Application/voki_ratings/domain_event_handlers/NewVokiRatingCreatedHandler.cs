using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.app_user_aggregate;
using VokiRatingsService.Domain.voki_aggregate;
using VokiRatingsService.Domain.voki_rating_aggregate.events;

namespace VokiRatingsService.Application.voki_ratings.domain_event_handlers;

internal class NewVokiRatingCreatedHandler : IDomainEventHandler<NewVokiRatingCreatedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IVokisRepository _vokisRepository;

    public NewVokiRatingCreatedHandler(IAppUsersRepository appUsersRepository, IVokisRepository vokisRepository) {
        _appUsersRepository = appUsersRepository;
        _vokisRepository = vokisRepository;
    }

    public async Task Handle(NewVokiRatingCreatedEvent e, CancellationToken ct) {
        Voki voki = (await _vokisRepository.GetById(e.VokiId))!;
        voki.AddRating(e.RatingId);
        await _vokisRepository.Update(voki, ct);

        AppUser user = (await _appUsersRepository.GetById(e.UserId))!;
        user.AddRating(e.RatingId);
        await _appUsersRepository.Update(user, ct);
    }
}