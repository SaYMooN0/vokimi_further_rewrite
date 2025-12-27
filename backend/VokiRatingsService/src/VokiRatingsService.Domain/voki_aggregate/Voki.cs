using SharedKernel;

namespace VokiRatingsService.Domain.voki_aggregate;

public class Voki : AggregateRoot<VokiId>
{
    private Voki() { }
    public ImmutableHashSet<VokiRatingId> RatingIds { get; private set; }
    private VokiManagersIdsSet ManagersSet { get; set; }
    private AppUserId PrimaryAuthorId { get; }

    public Voki(VokiId id, AppUserId primaryAuthorId, VokiManagersIdsSet managers) {
        Id = id;
        RatingIds = [];
        PrimaryAuthorId = primaryAuthorId;
        ManagersSet = managers;
    }

    public void AddRating(VokiRatingId vokiRatingId) {
        RatingIds = RatingIds.Add(vokiRatingId);
    }

    public bool CanUserManage(IAuthenticatedUserContext userContext) {
        AppUserId userId = userContext.UserId;
        return PrimaryAuthorId == userId || ManagersSet.Contains(userId);
    }
}