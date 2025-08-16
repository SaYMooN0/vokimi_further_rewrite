using SharedKernel.common.vokis;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.catalog;

public record class VokiOverviewResponse(
    string Id,
    VokiType Type,
    string Name,
    string Cover,
    string PrimaryAuthorId,
    string[] CoAuthorIds,
    string Description,
    bool IsAgeRestricted,
    Language Language,
    string[] Tags,
    uint RatingsCount,
    uint CommentsCount
)
{
    public static VokiOverviewResponse Create(BaseVoki v) => new(
        v.Id.ToString(),
        v.Type,
        v.Name.ToString(),
        v.Cover.ToString(),
        PrimaryAuthorId: v.PrimaryAuthorId.ToString(),
        CoAuthorIds: v.CoAuthorIds.Select(id => id.ToString()).ToArray(),
        v.Details.Description,
        v.Details.IsAgeRestricted,
        v.Details.Language,
        v.Tags.Select(t => t.ToString()).ToArray(),
        v.RatingsCount,
        v.CommentsCount
    );
}