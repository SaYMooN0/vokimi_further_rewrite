using DraftVokisLib;
using DraftVokisLib.events;
using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public sealed class DraftGeneralVoki : BaseDraftVoki
{
    private DraftGeneralVoki() { }

    private DraftGeneralVoki(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, VokiCoverPath coverPath,
        DateTime creationDate
    ) : base(
        vokiId, primaryAuthorId,
        name, coverPath,
        creationDate
    ) { }

    public static DraftGeneralVoki Create(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, VokiCoverPath coverPath,
        DateTime creationDate
    ) {
        DraftGeneralVoki newGeneralVoki = new(vokiId, primaryAuthorId, name, coverPath, creationDate);
        newGeneralVoki.AddDomainEvent(new NewDraftVokiInitializedEvent(newGeneralVoki.Id, newGeneralVoki.PrimaryAuthorId));
        return newGeneralVoki;
    }
}