using GeneralVokiTakingService.Domain.app_user_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate.events;

namespace GeneralVokiTakingService.Application.voki_taken_records.domain_event_handlers;

internal class VokiTakenRecordCreatedEventHandler : IDomainEventHandler<VokiTakenRecordCreatedEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IGeneralVokisRepository _generalVokisRepository;

    public VokiTakenRecordCreatedEventHandler(
        IAppUsersRepository appUsersRepository,
        IGeneralVokisRepository generalVokisRepository
    ) {
        _appUsersRepository = appUsersRepository;
        _generalVokisRepository = generalVokisRepository;
    }

    public async Task Handle(VokiTakenRecordCreatedEvent e, CancellationToken ct) {
        GeneralVoki voki = (await _generalVokisRepository.GetById(e.VokiId, ct))!;
        voki.AddNewVokiTakenRecord(e.VokiTakenRecordId, e.VokiTakerId);

        if (e.VokiTakerId is null) {
            return;
        }

        AppUser? vokiTaker = await _appUsersRepository.GetById(e.VokiTakerId, ct);
        if (vokiTaker is null) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.NotFound.User("Voki taker not found"));
        }

        vokiTaker!.VokiTaken(e.VokiTakenRecordId, e.ReceivedResultId);
        await _appUsersRepository.Update(vokiTaker, ct);
        await _generalVokisRepository.Update(voki, ct);
    }
}