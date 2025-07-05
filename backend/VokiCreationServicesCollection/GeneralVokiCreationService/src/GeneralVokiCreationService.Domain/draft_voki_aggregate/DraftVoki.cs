using DraftVokisLib;
using DraftVokisLib.events;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Domain.draft_voki_aggregate;

public sealed class DraftVoki : BaseDraftVoki
{
    private DraftVoki() : base() { }

    private DraftVoki(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, VokiCoverPath coverPath,
        DateTime creationDate
    ) : base(
        vokiId, primaryAuthorId,
        name, coverPath,
        creationDate
    ) { }

    public static DraftVoki Create(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, VokiCoverPath coverPath,
        DateTime creationDate
    ) {
        DraftVoki newVoki = new(vokiId, primaryAuthorId, name, coverPath, creationDate);
        newVoki.AddDomainEvent(new NewDraftVokiInitializedEvent(newVoki.Id, newVoki.PrimaryAuthorId));
        return newVoki;
    }
}