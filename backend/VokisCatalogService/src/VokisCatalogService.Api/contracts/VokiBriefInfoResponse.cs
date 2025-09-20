using SharedKernel.common.vokis;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.contracts;

internal sealed record class VokiBriefInfoResponse(
    string Id,
    VokiType Type,
    string Name,
    string Cover,
    string PrimaryAuthorId,
    string[] CoAuthorIds,
    bool HasMatureContent,
    Language Language
)
{
    public static VokiBriefInfoResponse Create(BaseVoki v) => new(
        v.Id.ToString(),
        v.Type,
        v.Name.ToString(),
        v.Cover.ToString(),
        v.PrimaryAuthorId.ToString(),
        v.CoAuthorIds.Select(id => id.ToString()).ToArray(),
        v.Details.HasMatureContent,
        v.Details.Language
    );
}