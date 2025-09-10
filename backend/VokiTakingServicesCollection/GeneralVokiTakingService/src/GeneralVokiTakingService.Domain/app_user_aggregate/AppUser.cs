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

    public void AddVokiTakenRecordId(VokiTakenRecordId id) {
        GeneralVokiTakenRecordIds = GeneralVokiTakenRecordIds.Add(id);
    }
}