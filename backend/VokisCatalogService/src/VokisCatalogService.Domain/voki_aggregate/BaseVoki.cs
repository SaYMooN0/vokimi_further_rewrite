namespace VokisCatalogService.Domain.voki_aggregate;

public abstract class BaseVoki : AggregateRoot<VokiId>
{
    protected BaseVoki() { }
    public abstract VokiType VokiType { get; }
    public VokiName Name { get; }
    public ImmutableHashSet<VokiTagId> Tags { get; }
    public AppUserId PrimaryAuthorId { get; }
    public ImmutableHashSet<AppUserId> CoAuthorIds { get; }
    public uint LikesCount { get; private set; }
    public uint CommentsCount { get; private set; }
    public uint VokiTakingsCount { get; private set; }

    protected BaseVoki(
        VokiId id, VokiName name,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds,
        ImmutableHashSet<VokiTagId> tags
    ) {
        Id = id;
        Name = name;
        PrimaryAuthorId = primaryAuthorId;
        CoAuthorIds = coAuthorIds;
        Tags = tags;
        LikesCount = 0;
        CommentsCount = 0;
        VokiTakingsCount = 0;
    }
}