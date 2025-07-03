using DraftVokisLib;
using DraftVokisLib.events;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Domain.draft_voki_aggregate;

public class DraftVoki : BaseDraftVoki
{
    protected DraftVoki() : base() { }

    protected DraftVoki(VokiId vokiId, AppUserId primaryAuthorId, VokiName vokiName, DateTime creationDate) : base(
        vokiId,
        primaryAuthorId,
        vokiName,
        creationDate
    ) { }

    public static DraftVoki Create(VokiId vokiId, AppUserId primaryAuthorId, VokiName vokiName, DateTime creationDate) {
        DraftVoki newVoki = new(vokiId, primaryAuthorId, vokiName, creationDate);
        newVoki.AddDomainEvent(new NewDraftVokiInitializedEvent(newVoki.Id));
        return newVoki;
    }
}