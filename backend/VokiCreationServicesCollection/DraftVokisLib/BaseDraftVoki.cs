using SharedKernel.common.vokis;

namespace DraftVokisLib;

public abstract class BaseDraftVoki : AggregateRoot<VokiId>
{
    protected BaseDraftVoki() { }
    protected AppUserId PrimaryAuthorId { get; }
    protected VokiCoAuthorIdsSet CoAuthors { get; private set; }
    public VokiName VokiName { get; private set; }

    protected BaseDraftVoki(VokiId vokiId, AppUserId primaryAuthorId, VokiName vokiName, DateTime creationDate) {
        Id = vokiId;
        PrimaryAuthorId = primaryAuthorId;
        VokiName = vokiName;
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