using CoreVokiCreationService.Domain.app_user_aggregate.events;
using CoreVokiCreationService.Domain.common.interfaces.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.domain_event_handlers;

internal class CoAuthorInviteDeclinedEventHandler : IDomainEventHandler<CoAuthorInviteDeclinedEvent>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    public CoAuthorInviteDeclinedEventHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }

    public async Task Handle(CoAuthorInviteDeclinedEvent e, CancellationToken ct) {
        DraftVoki voki = (await _draftVokiRepository.GetById(e.VokiId))!;
        voki.DeclineCoAuthorInvite(e.AppUserId);
        await _draftVokiRepository.Update(voki);
    }
}