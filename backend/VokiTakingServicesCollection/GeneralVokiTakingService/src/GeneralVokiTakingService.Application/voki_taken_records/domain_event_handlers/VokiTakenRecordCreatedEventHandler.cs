using GeneralVokiTakingService.Domain.app_user_aggregate;
using GeneralVokiTakingService.Domain.common.interfaces.repositories;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate.events;

namespace GeneralVokiTakingService.Application.voki_taken_records.domain_event_handlers;

internal class VokiTakenRecordCreatedEventHandler : IDomainEventHandler<VokiTakenRecordCreatedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public VokiTakenRecordCreatedEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(VokiTakenRecordCreatedEvent e, CancellationToken ct) {
        if (e.VokiTakerId is null) {
            return;
        }

        AppUser? vokiTaker = await _appUsersRepository.GetById(e.VokiTakerId, ct);
        if (vokiTaker is null) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.NotFound.User("Voki taker not found"));
        }

        vokiTaker!.VokiTaken(e.VokiTakenRecordId, e.ReceivedResultId);
        await _appUsersRepository.Update(vokiTaker, ct);
    }
}