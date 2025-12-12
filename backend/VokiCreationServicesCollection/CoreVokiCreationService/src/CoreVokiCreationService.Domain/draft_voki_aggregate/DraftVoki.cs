using CoreVokiCreationService.Domain.draft_voki_aggregate.events;
using SharedKernel;
using SharedKernel.common.rules;
using SharedKernel.common.vokis;
using VokimiStorageKeysLib.concrete_keys;

namespace CoreVokiCreationService.Domain.draft_voki_aggregate;

public class DraftVoki : AggregateRoot<VokiId>
{
    private DraftVoki() { }
    public VokiType Type { get; }
    public VokiName Name { get; private set; }
    public VokiCoverKey Cover { get; private set; }
    public AppUserId PrimaryAuthorId { get; }
    public ImmutableHashSet<AppUserId> CoAuthorIds { get; private set; }
    public VokiExpectedManagersSetting ExpectedManagers { get; private set; }
    public ImmutableHashSet<AppUserId> InvitedForCoAuthorUserIds { get; private set; }
    public DateTime CreationDate { get; }

    private DraftVoki(
        VokiId id, VokiType type, VokiName name, VokiCoverKey cover,
        AppUserId primaryAuthorId, DateTime creationDate
    ) {
        Id = id;
        Type = type;
        Name = name;
        Cover = cover;
        PrimaryAuthorId = primaryAuthorId;
        CreationDate = creationDate;
        CoAuthorIds = [];
        InvitedForCoAuthorUserIds = [];
    }

    public static DraftVoki Create(VokiName name, VokiType type, AppUserId primaryAuthorId, DateTime creationDate) {
        VokiId vokiId = VokiId.CreateNew();
        VokiCoverKey cover = VokiCoverKey.CreateWithId(vokiId, CommonStorageItemKey.DefaultVokiCover.ImageExtension);
        DraftVoki newVoki = new(vokiId, type, name, cover, primaryAuthorId, creationDate);
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

    public ErrOrNothing InviteCoAuthors(ImmutableHashSet<AppUserId> userIdsToInvite) {
        if (userIdsToInvite.Count == 0) {
            return ErrOrNothing.Nothing;
        }

        if (userIdsToInvite.Contains(PrimaryAuthorId)) {
            return ErrFactory.Conflict("You cannot invite the primary author of this Voki to be a co-author");
        }

        ImmutableHashSet<AppUserId> newInvitedNotAlreadyCoAuthors = userIdsToInvite.Except(CoAuthorIds);

        if (newInvitedNotAlreadyCoAuthors.Count == 0) {
            return ErrFactory.Conflict("All selected users are already co-authors");
        }

        ImmutableHashSet<AppUserId> uniqueNewInvites = newInvitedNotAlreadyCoAuthors.Except(InvitedForCoAuthorUserIds);

        if (uniqueNewInvites.Count == 0) {
            return ErrOrNothing.Nothing;
        }

        int totalAfterInvites = CoAuthorIds.Count + InvitedForCoAuthorUserIds.Count + uniqueNewInvites.Count;
        if (totalAfterInvites > VokiRules.MaxCoAuthors) {
            return ErrFactory.LimitExceeded(
                $"Voki cannot have more than {VokiRules.MaxCoAuthors} co-authors (including invited)"
            );
        }

        InvitedForCoAuthorUserIds = InvitedForCoAuthorUserIds.Union(uniqueNewInvites);
        AddDomainEvent(new NewUsersInvitedForCoAuthorEvent(uniqueNewInvites, this.Id));
        return ErrOrNothing.Nothing;
    }


    public ErrOrNothing AcceptInviteBy(AppUserId userId) {
        if (CoAuthorIds.Contains(userId)) {
            return ErrOrNothing.Nothing;
        }

        if (PrimaryAuthorId == userId) {
            return ErrFactory.Conflict("Primary author cannot become a co-author");
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
        AddDomainEvent(new CoAuthorInviteAcceptedEvent(Id, userId, Type));
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

    // public ErrOrNothing DropCoAuthor() { }
    public void MarkAsPublished() {
        foreach (var invitedUserIds in InvitedForCoAuthorUserIds) {
            AddDomainEvent(new CoAuthorInviteCanceledEvent(Id, invitedUserIds));
        }

        AddDomainEvent(new VokiPublishedEvent(Id, PrimaryAuthorId, CoAuthorIds));
    }

    public void DropCoAuthor(AppUserId coAuthorId) {
        if (!CoAuthorIds.Contains(coAuthorId)) {
            return;
        }

        CoAuthorIds = CoAuthorIds.Remove(coAuthorId);
        AddDomainEvent(new VokiCoAuthorRemovedEvent(Id, coAuthorId, this.Type));
    }
    public void LeaveVokiCreation(IAuthenticatedUserContext userContext) {
        if (!CoAuthorIds.Contains(userContext.UserId)) {
            return;
        }
        CoAuthorIds = CoAuthorIds.Remove(userContext.UserId);
        AddDomainEvent(new VokiCoAuthorRemovedEvent(Id, userContext.UserId, this.Type));
    }
}