using VokimiStorageKeysLib.concrete_keys;

namespace VokisCatalogService.Domain.voki_aggregate;

public abstract class BaseVoki : AggregateRoot<VokiId>
{
    protected BaseVoki() { }
    public abstract VokiType Type { get; }
    public VokiName Name { get; }
    public VokiCoverKey Cover { get; }
    public AppUserId PrimaryAuthorId { get; }
    public ImmutableHashSet<AppUserId> CoAuthorIds { get; }
    public VokiDetails Details { get; }
    public ImmutableHashSet<VokiTagId> Tags { get; }
    public DateTime PublicationDate { get; }
    public uint RatingsCount { get; private set; }
    public uint CommentsCount { get; private set; }
    public uint VokiTakingsCount { get; private set; }


    protected BaseVoki(
        VokiId id, VokiName name, VokiCoverKey cover,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds,
        VokiDetails details, ImmutableHashSet<VokiTagId> tags, DateTime publicationDate
    ) {
        Id = id;
        Name = name;
        Cover = cover;
        PrimaryAuthorId = primaryAuthorId;
        CoAuthorIds = coAuthorIds;
        Details = details;
        Tags = tags;
        RatingsCount = 0;
        CommentsCount = 0;
        VokiTakingsCount = 0;
        PublicationDate = publicationDate;
    }
}