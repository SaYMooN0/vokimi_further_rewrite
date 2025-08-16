using CoreVokiCreationService.Domain.draft_voki_aggregate;
using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Api.contracts.vokis_brief_info;

internal sealed record class VokiBriefInfoResponse(
    string Id,
    VokiType Type,
    string Name,
    string Cover,
    string PrimaryAuthorId,
    string[] CoAuthorIds
)
{
    public static VokiBriefInfoResponse Create(DraftVoki v) => new(
        v.Id.ToString(),
        v.Type,
        v.Name.ToString(),
        v.Cover.ToString(),
        v.PrimaryAuthorId.ToString(),
        v.CoAuthorIds.Select(id => id.ToString()).ToArray()
    );
}