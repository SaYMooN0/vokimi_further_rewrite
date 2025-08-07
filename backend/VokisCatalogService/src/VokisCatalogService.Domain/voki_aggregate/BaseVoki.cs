namespace VokisCatalogService.Domain.voki_aggregate;

public abstract class BaseVoki : AggregateRoot<VokiId>
{
    protected BaseVoki() { }
    public AppUserId PrimaryAuthorId { get; }
    public ImmutableHashSet<AppUserId> CoAuthorIds { get; }

    protected BaseVoki(VokiId id, AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds) {
        PrimaryAuthorId = primaryAuthorId;
        CoAuthorIds = coAuthorIds;
        Id = id;
    }
}