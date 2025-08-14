using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.common.interfaces.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate.events;

namespace CoreVokiCreationService.Application.app_users.domain_event_handlers;

internal class VokiPublishedEventHandler : IDomainEventHandler<VokiPublishedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public VokiPublishedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(VokiPublishedEvent e, CancellationToken ct) {
        List<AppUser> usersTpUpdate = [];

        AppUser? initiator = await _appUsersRepository.GetById(e.PrimaryAuthorId);
        if (initiator is not null) {
            initiator.RemoveInitializedVoki(e.VokiId);
            usersTpUpdate.Add(initiator);
        }

        foreach (var coAuthorsId in e.CoAuthorsIds) {
            AppUser? coauthor = await _appUsersRepository.GetById(coAuthorsId)!;
            if (coauthor is not null) {
                coauthor.RemoveCoAuthoredVoki(e.VokiId);
                usersTpUpdate.Add(coauthor);
            }
        }

        if (usersTpUpdate.Count > 0) {
            await _appUsersRepository.UpdateRange(usersTpUpdate);
        }
    }
}