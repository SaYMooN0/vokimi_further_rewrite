using SharedKernel.common.vokis;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.events;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.publishing;
using VokimiStorageKeysLib.concrete_keys;

namespace VokiCreationServicesLib.Domain.draft_voki_aggregate;

public abstract class BaseDraftVoki : AggregateRoot<VokiId>
{
    protected BaseDraftVoki() { }
    public AppUserId PrimaryAuthorId { get; }
    protected VokiCoAuthorIdsSet CoAuthors { get; private set; }
    public VokiName Name { get; private set; }
    public VokiCoverKey Cover { get; private set; }
    public VokiDetails Details { get; private set; }
    public VokiTagsSet Tags { get; private set; }
    protected abstract IVokiInteractionSettings BaseInteractionSettings { get; }
    public DateTime CreationDate { get; }

    protected BaseDraftVoki(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, VokiCoverKey cover,
        DateTime creationDate
    ) {
        Id = vokiId;
        PrimaryAuthorId = primaryAuthorId;
        CoAuthors = VokiCoAuthorIdsSet.Empty;

        Name = name;
        Cover = cover;
        Details = VokiDetails.Default;
        Tags = VokiTagsSet.Empty;

        CreationDate = creationDate;
    }

    public ErrOrNothing AddCoAuthor(AppUserId newCoAuthorId) {
        if (newCoAuthorId == PrimaryAuthorId) {
            return ErrFactory.Conflict("Primary author cannot be a co-author of the Voki");
        }

        ErrOr<VokiCoAuthorIdsSet> newSetRes = CoAuthors.Add(newCoAuthorId);
        if (newSetRes.IsErr(out var err)) {
            return err;
        }

        CoAuthors = newSetRes.AsSuccess();
        return ErrOrNothing.Nothing;
    }
    public ErrOrNothing RemoveCoAuthor(AppUserId coAuthorId)
    {
        if (coAuthorId == PrimaryAuthorId) {
            return ErrFactory.Conflict("Primary author cannot be removed from co-authors");
        }

        ErrOr<VokiCoAuthorIdsSet> newSetRes = CoAuthors.Remove(coAuthorId);
        if (newSetRes.IsErr(out var err)) {
            return err;
        }

        CoAuthors = newSetRes.AsSuccess();
        return ErrOrNothing.Nothing;
    }
    public bool HasAccessToEdit(AppUserId userId) =>
        userId == PrimaryAuthorId || CoAuthors.Contains(userId);

    public void UpdateName(VokiName newVokiName) {
        Name = newVokiName;
        AddDomainEvent(new VokiNameUpdatedEvent(Id, Name));
    }

    public ErrOrNothing UpdateCover(VokiCoverKey newCover) {
        if (!newCover.IsWithId(this.Id)) {
            return ErrFactory.Conflict(
                "This cover does not belong to this Voki", $"Voki id: {Id}, cover voki id: {newCover.VokiId}"
            );
        }

        VokiCoverKey oldCover = this.Cover;
        this.Cover = newCover;
        AddDomainEvent(new VokiCoverUpdatedEvent(Id, OldCover: oldCover, Cover));
        return ErrOrNothing.Nothing;
    }

    public void UpdateDetails(VokiDetails newDetails) {
        this.Details = newDetails;
    }

    public void UpdateTags(VokiTagsSet newTags) {
        this.Tags = newTags;
    }

    protected List<VokiPublishingIssue> CheckCoverForPublishingIssues() {
        if (!Cover.IsWithId(Id)) {
            return [
                VokiPublishingIssue.Problem(
                    message: "Cover does not belong to this Voki",
                    source: "Cover",
                    fixRecommendation: "Try choosing another cover"
                )
            ];
        }

        return [];
    }

    protected List<VokiPublishingIssue> CheckDetailsForPublishingIssues() {
        if (string.IsNullOrWhiteSpace(Details.Description.ToString())) {
            return [
                VokiPublishingIssue.Warning(
                    message: "Description is empty",
                    source: "Description",
                    fixRecommendation: "Add a description to help users understand your Voki"
                )
            ];
        }

        return [];
    }

    protected List<VokiPublishingIssue> CheckTagsForPublishingIssues() {
        if (Tags.Value.Count == 0) {
            return [
                VokiPublishingIssue.Warning(
                    message: "No tags have been added",
                    source: "Tags",
                    fixRecommendation: "Add some tags so users can discover your Voki more easily"
                )
            ];
        }

        return [];
    }
}