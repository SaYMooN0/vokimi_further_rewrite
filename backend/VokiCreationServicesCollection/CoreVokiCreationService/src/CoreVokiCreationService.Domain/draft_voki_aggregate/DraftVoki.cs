using CoreVokiCreationService.Domain.draft_voki_aggregate.events;
using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate;

public class DraftVoki : AggregateRoot<VokiId>
{
    private DraftVoki() { }
    public VokiType Type { get; }
    public VokiName Name { get; private set; }
    public VokiCoverPath CoverPath { get; private set; } = new VokiCoverPath();
    public AppUserId PrimaryAuthorId { get; }
    public ImmutableHashSet<AppUserId> CoAuthorsIds { get; private set; }
    public ImmutableHashSet<AppUserId> InvitedForCoAuthorUserIds { get; private set; }
    public DateTime CreationDate { get; }

    private DraftVoki(VokiId id, VokiType type, VokiName name, AppUserId primaryAuthorId, DateTime creationDate) {
        Id = id;
        Type = type;
        Name = name;
        CoverPath = VokiCoverPath.Default;
        PrimaryAuthorId = primaryAuthorId;
        CreationDate = creationDate;
        CoAuthorsIds = [];
        InvitedForCoAuthorUserIds = [];
    }

    public static DraftVoki Create(VokiName name, VokiType type, AppUserId primaryAuthorId, DateTime creationDate) {
        DraftVoki newVoki = new(VokiId.CreateNew(), type, name, primaryAuthorId, creationDate);
        newVoki.AddDomainEvent(new NewDraftVokiInitializedEvent(
            newVoki.Id,
            newVoki.Type,
            newVoki.Name,
            newVoki.CoverPath,
            newVoki.PrimaryAuthorId,
            newVoki.CreationDate
        ));
        return newVoki;
    }

    // public ErrOrNothing InviteNewCoAuthor() { }
    // public ErrOrNothing CancelCoAuthorInvite() { }
    // public ErrOrNothing AddCoAuthor() { }
    // public ErrOrNothing RemoveCoAuthor() { }
    public bool HasAccessToEdit(AppUserId userId) =>
        userId == PrimaryAuthorId || CoAuthorsIds.Contains(userId);
}