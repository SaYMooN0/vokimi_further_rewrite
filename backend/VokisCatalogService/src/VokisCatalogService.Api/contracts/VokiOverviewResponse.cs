using SharedKernel.common.vokis;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.contracts;

public record class VokiOverviewResponse(
    string Id,
    VokiType Type,
    string Name,
    string Cover,
    string PrimaryAuthorId,
    string[] CoAuthorIds,
    string Description,
    bool HasMatureContent,
    Language Language,
    string[] Tags,
    DateTime PublicationDate,
    uint RatingsCount,
    uint CommentsCount,
    bool SignedInOnlyTaking
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
        v.Details.HasMatureContent,
        v.Details.Language,
        v.Tags.Select(t => t.ToString()).ToArray(),
        v.PublicationDate,
        v.RatingsCount,
        v.CommentsCount,
        v.SignedInOnlyTaking
    );
}