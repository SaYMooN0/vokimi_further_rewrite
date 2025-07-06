using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Api.contracts.vokis_brief_info;

public record class VokiBriefInfoResponse(
    string Id,
    VokiType Type,
    string Name,
    string Cover,
    string PrimaryAuthorId,
    int CoAuthorsCount
)
{
    public static VokiBriefInfoResponse FromVoki(DraftVoki v) => new(
        v.Id.ToString(),
        v.Type,
        v.Name.ToString(),
        v.CoverPath.ToString(),
        v.PrimaryAuthorId.ToString(),
        v.CoAuthorsIds.Count
    );
}