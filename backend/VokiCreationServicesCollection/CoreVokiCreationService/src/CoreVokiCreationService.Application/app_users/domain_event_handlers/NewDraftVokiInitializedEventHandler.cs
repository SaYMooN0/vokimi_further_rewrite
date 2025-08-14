using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.common.interfaces.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate.events;

namespace CoreVokiCreationService.Application.app_users.domain_event_handlers;

internal class NewDraftVokiInitializedEventHandler : IDomainEventHandler<NewDraftVokiInitializedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public NewDraftVokiInitializedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(NewDraftVokiInitializedEvent e, CancellationToken ct) {
        AppUser initiator = (await _appUsersRepository.GetById(e.PrimaryAuthorId))!;
        initiator.AddInitializedVoki(e.VokiId);
        await _appUsersRepository.Update(initiator);
    }
}