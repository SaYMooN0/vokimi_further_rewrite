using SharedKernel.common.vokis;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.events;
using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public sealed class DraftGeneralVoki : BaseDraftVoki
{
    private DraftGeneralVoki() { }

    private DraftGeneralVoki(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, DraftVokiCoverKey cover,
        DateTime creationDate
    ) : base(
        vokiId, primaryAuthorId,
        name, cover,
        creationDate
    ) { }

    public static DraftGeneralVoki Create(
        VokiId vokiId, AppUserId primaryAuthorId,
        VokiName name, DraftVokiCoverKey cover,
        DateTime creationDate
    ) {
        DraftGeneralVoki newGeneralVoki = new(vokiId, primaryAuthorId, name, cover, creationDate);
        newGeneralVoki.AddDomainEvent(
            new NewDraftVokiInitializedEvent(newGeneralVoki.Id, newGeneralVoki.PrimaryAuthorId));
        return newGeneralVoki;
    }

}