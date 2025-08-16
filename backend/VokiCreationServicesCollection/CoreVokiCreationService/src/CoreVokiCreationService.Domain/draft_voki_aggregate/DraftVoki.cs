using CoreVokiCreationService.Domain.app_user_aggregate.events;
using CoreVokiCreationService.Domain.draft_voki_aggregate.events;
using SharedKernel.common.rules;
using SharedKernel.common.vokis;
using VokimiStorageKeysLib.voki_cover;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate;

public class DraftVoki : AggregateRoot<VokiId>
{
    private DraftVoki() { }
    public VokiType Type { get; }
    public VokiName Name { get; private set; }
    public VokiCoverKey Cover { get; private set; }
    public AppUserId PrimaryAuthorId { get; }
    public ImmutableHashSet<AppUserId> CoAuthorIds { get; private set; }
    public ImmutableHashSet<AppUserId> InvitedForCoAuthorUserIds { get; private set; }
    public DateTime CreationDate { get; }

    private DraftVoki(VokiId id, VokiType type, VokiName name, AppUserId primaryAuthorId, DateTime creationDate) {
        Id = id;
        Type = type;
        Name = name;
        Cover = VokiCoverKey.Default;
        PrimaryAuthorId = primaryAuthorId;
        CreationDate = creationDate;
        CoAuthorIds = [];
        InvitedForCoAuthorUserIds = [];
    }

    public static DraftVoki Create(VokiName name, VokiType type, AppUserId primaryAuthorId, DateTime creationDate) {
        DraftVoki newVoki = new(VokiId.CreateNew(), type, name, primaryAuthorId, creationDate);
        newVoki.AddDomainEvent(new NewDraftVokiInitializedEvent(
            newVoki.Id,
            newVoki.Type,
            newVoki.Name,
            newVoki.Cover,
            newVoki.PrimaryAuthorId,
            newVoki.CreationDate
        ));
        return newVoki;
    }


    public bool HasAccessToEdit(AppUserId userId) =>
        userId == PrimaryAuthorId || CoAuthorIds.Contains(userId);

    public ErrOrNothing UpdateCover(VokiCoverKey newCover) {
        if (!newCover.IsWithId(this.Id)) {
            return ErrFactory.Conflict(
                "This cover does not belong to this Voki", $"Voki id: {Id}, cover voki id: {newCover.VokiId}"
            );
        }

        this.Cover = newCover;
        return ErrOrNothing.Nothing;
    }

    public void UpdateName(VokiName newName) {
        Name = newName;
    }

    public ErrOrNothing InviteNewCoAuthor(AppUserId invitedUserId) {
        if (PrimaryAuthorId == invitedUserId) {
            return ErrFactory.Conflict("You cannot invite primary author of voki to be a co-author");
        }

        if (InvitedForCoAuthorUserIds.Contains(invitedUserId)) {
            return ErrOrNothing.Nothing;
        }

        int totalCoAuthors = CoAuthorIds.Count + InvitedForCoAuthorUserIds.Count;
        if (totalCoAuthors >= VokiRules.MaxCoAuthors) {
            return ErrFactory.LimitExceeded($"Voki cannot have more than {VokiRules.MaxCoAuthors} co-authors");
        }

        InvitedForCoAuthorUserIds = InvitedForCoAuthorUserIds.Add(invitedUserId);
        AddDomainEvent(new CoAuthorInviteCreatedEvent(invitedUserId, this.Id));
        return ErrOrNothing.Nothing;
    }

    public ErrOrNothing AcceptInviteBy(AppUserId userId) {
        if (CoAuthorIds.Contains(userId)) {
            return ErrOrNothing.Nothing;
        }

        if (!InvitedForCoAuthorUserIds.Contains(userId)) {
            return ErrFactory.Unspecified("You are not listed as invited in this voki");
        }

        if (CoAuthorIds.Count >= VokiRules.MaxCoAuthors) {
            return ErrFactory.Conflict(
                $"Some error has occured. This voki already has {VokiRules.MaxCoAuthors} co-authors"
            );
        }

        CoAuthorIds = CoAuthorIds.Add(userId);
        InvitedForCoAuthorUserIds = InvitedForCoAuthorUserIds.Remove(userId);
        AddDomainEvent(new CoAuthorInviteAcceptedEvent(Id, userId));
        return ErrOrNothing.Nothing;
    }

    public void CancelCoAuthorInvite(AppUserId userId) {
        if (!InvitedForCoAuthorUserIds.Contains(userId)) {
            return;
        }

        InvitedForCoAuthorUserIds = InvitedForCoAuthorUserIds.Remove(userId);
        AddDomainEvent(new CoAuthorInviteCanceledEvent(Id, userId));
    }

    public void DeclineCoAuthorInvite(AppUserId userId) {
        InvitedForCoAuthorUserIds = InvitedForCoAuthorUserIds.Remove(userId);
    }

    // public ErrOrNothing RemoveCoAuthor() { }
    public void MarkAsPublished() {
        foreach (var invitedUserIds in InvitedForCoAuthorUserIds) {
            AddDomainEvent(new CoAuthorInviteCanceledEvent(Id, invitedUserIds));
        }

        AddDomainEvent(new VokiPublishedEvent(Id, PrimaryAuthorId, CoAuthorIds));
    }
}