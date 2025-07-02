namespace GeneralVokiCreationService.Domain.draft_voki_aggregate;

public class DraftVoki : AggregateRoot<VokiId>
{
    private DraftVoki() { }
    private AppUserId PrimaryAuthorId { get; }
    public VokiCoAuthorIdsSet CoAuthors { get; private set; }

    private DraftVoki(VokiId vokiId, AppUserId primaryAuthorId) {
        Id = vokiId;
        PrimaryAuthorId = primaryAuthorId;
        CoAuthors = VokiCoAuthorIdsSet.Empty;
    }

    public ErrOrNothing UpdateCoAuthorsIds(VokiCoAuthorIdsSet newCoAuthorsSet) {
        if (newCoAuthorsSet.Contains(this.PrimaryAuthorId)) {
            return ErrFactory.Conflict("Primary author cannot be specified as co-author");
        }

        CoAuthors = newCoAuthorsSet;
        return ErrOrNothing.Nothing;
    }
}