using SharedKernel.common.vokis;

namespace DraftVokisLib;

public abstract class BaseDraftVoki : AggregateRoot<VokiId>
{
    protected BaseDraftVoki() { }
    public AppUserId PrimaryAuthorId { get; }
    protected VokiCoAuthorIdsSet CoAuthors { get; private set; }
    public VokiName Name { get; private set; }
    public VokiCoverPath CoverPath { get; private set; }
    public DateTime CreationDate { get; }


    protected BaseDraftVoki(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, VokiCoverPath coverPath,
        DateTime creationDate
        ) {
        Id = vokiId;
        PrimaryAuthorId = primaryAuthorId;
        Name = name;
        CoverPath = coverPath;
        CoAuthors = VokiCoAuthorIdsSet.Empty;
        CreationDate = creationDate;
    }

    public ErrOrNothing UpdateCoAuthorsIds(VokiCoAuthorIdsSet newCoAuthorsSet) {
        if (newCoAuthorsSet.Contains(this.PrimaryAuthorId)) {
            return ErrFactory.Conflict("Primary author cannot be specified as co-author");
        }

        CoAuthors = newCoAuthorsSet;
        return ErrOrNothing.Nothing;
    }
}