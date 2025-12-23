using VokimiStorageKeysLib.concrete_keys;
using VokisCatalogService.Domain.voki_aggregate.voki_types;

namespace VokisCatalogService.Domain.voki_aggregate;

public abstract class BaseVoki : AggregateRoot<VokiId>
{
    protected BaseVoki() { }
    public abstract VokiType Type { get; }
    public VokiName Name { get; }
    public VokiCoverKey Cover { get; }
    public AppUserId PrimaryAuthorId { get; }
    public ImmutableHashSet<AppUserId> CoAuthorIds { get; }
    public VokiManagersIdsSet ManagersSet { get; private set; }
    public VokiDetails Details { get; }
    public ImmutableHashSet<VokiTagId> Tags { get; }
    public DateTime PublicationDate { get; }
    public uint RatingsCount { get; private set; }
    public uint CommentsCount { get; private set; }
    public uint VokiTakingsCount { get; private set; }
    public abstract IVokiInteractionSettings BaseInteractionSettings { get; }
    public CatalogPageSettings CatalogPageSettings { get; }

    protected BaseVoki(
        VokiId id, VokiName name, VokiCoverKey cover,
        AppUserId primaryAuthorId, ImmutableHashSet<AppUserId> coAuthorIds, VokiManagersIdsSet managers,
        VokiDetails details, ImmutableHashSet<VokiTagId> tags, DateTime publicationDate
    ) {
        Id = id;
        Name = name;
        Cover = cover;
        PrimaryAuthorId = primaryAuthorId;
        CoAuthorIds = coAuthorIds;
        ManagersSet = managers;
        Details = details;
        Tags = tags;
        RatingsCount = 0;
        CommentsCount = 0;
        VokiTakingsCount = 0;
        PublicationDate = publicationDate;
    }

    public void UpdateVokiTakingsCount(uint newVokiTakingsCount) {
        if (newVokiTakingsCount > VokiTakingsCount) {
            VokiTakingsCount = newVokiTakingsCount;
        }
    }

    public void UpdateRatingsCount(uint newRatingsCount) {
        if (newRatingsCount > RatingsCount) {
            RatingsCount = newRatingsCount;
        }
    }
}

public static class BaseVokiExtensions
{
    public static TResult MatchOnType<TResult>(
        this BaseVoki v,
        Func<GeneralVoki, TResult> onGeneral,
        Func<TierListVoki, TResult> onTierList,
        Func<ScoringVoki, TResult> onScoring
    ) => v.Type.Match(
        onGeneral: () => onGeneral((v as GeneralVoki)!),
        onTierList: () => onTierList((v as TierListVoki)!),
        onScoring: () => onScoring((v as ScoringVoki)!)
    );
}