using GeneralVokiCreationService.Domain.app_user_aggregate;
using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;

namespace GeneralVokiCreationService.Application.app_users.domain_event_handlers;

internal class GeneralVokiPublishedEventHandler : IDomainEventHandler<GeneralVokiPublishedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public GeneralVokiPublishedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(GeneralVokiPublishedEvent e, CancellationToken ct) {
        List<AppUser> usersToUpdate = [];

        var initiator = await _appUsersRepository.GetById(e.PrimaryAuthorId);
        if (initiator is not null) {
            initiator.RemoveInitializedVoki(e.VokiId);
            usersToUpdate.Add(initiator);
        }

        foreach (var coAuthorId in e.CoAuthors.ToArray()) {
            var coAuthor = await _appUsersRepository.GetById(coAuthorId);
            if (coAuthor is null) continue;

            coAuthor.RemoveCoAuthoredVoki(e.VokiId);
            usersToUpdate.Add(coAuthor);
        }

        await _appUsersRepository.UpdateRange(usersToUpdate);
    }
}