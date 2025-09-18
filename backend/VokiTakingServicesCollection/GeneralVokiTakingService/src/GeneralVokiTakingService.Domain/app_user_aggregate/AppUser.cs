using GeneralVokiTakingService.Domain.general_voki_aggregate;
using VokiTakingServicesLib.Domain.common;

namespace GeneralVokiTakingService.Domain.app_user_aggregate;

public class AppUser : AggregateRoot<AppUserId>
{
    private AppUser() { }

    public AppUser(AppUserId id) {
        Id = id;
        GeneralVokiTakenRecordIds = [];
    }

    public ImmutableHashSet<VokiTakenRecordId> GeneralVokiTakenRecordIds { get; private set; }
    private ImmutableHashSet<GeneralVokiResultId> ReceivedResultIds { get; set; }

    public void VokiTaken(VokiTakenRecordId id, GeneralVokiResultId receivedResultId) {
        GeneralVokiTakenRecordIds = GeneralVokiTakenRecordIds.Add(id);
        ReceivedResultIds = ReceivedResultIds.Add(receivedResultId);
    }
}