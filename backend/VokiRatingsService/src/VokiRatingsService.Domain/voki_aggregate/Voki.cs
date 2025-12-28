using SharedKernel;
namespace VokiRatingsService.Domain.voki_aggregate;

public class Voki : AggregateRoot<VokiId>
{
    private Voki() { }
    private VokiManagersIdsSet ManagersSet { get; set; }
    private AppUserId PrimaryAuthorId { get; }
    private DateTime PublicationDate { get; }
    public Voki(VokiId id, AppUserId primaryAuthorId, VokiManagersIdsSet managers, DateTime publicationDate) {
        Id = id;
        PrimaryAuthorId = primaryAuthorId;
        ManagersSet = managers;
        PublicationDate = publicationDate;
    }
    public bool CanUserManage(IAuthenticatedUserContext userContext) {
        AppUserId userId = userContext.UserId;
        return PrimaryAuthorId == userId || ManagersSet.Contains(userId);
    }
}