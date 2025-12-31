using VokiRatingsService.Application.vokis.queries;

namespace VokiRatingsService.Api.contracts.manage_voki;

public record ManageVokiRatingsOverviewResponse(
    uint Rating1Count,
    uint Rating2Count,
    uint Rating3Count,
    uint Rating4Count,
    uint Rating5Count,
    DateTime VokiPublicationDate
) : ICreatableResponse<ManageVokiRatingsOverviewQueryResult>
{
    public static ICreatableResponse<ManageVokiRatingsOverviewQueryResult> Create(ManageVokiRatingsOverviewQueryResult r)
        => new ManageVokiRatingsOverviewResponse(
            Rating1Count: r.Distribution.Rating1Count,
            Rating2Count: r.Distribution.Rating2Count,
            Rating3Count: r.Distribution.Rating3Count,
            Rating4Count: r.Distribution.Rating4Count,
            Rating5Count: r.Distribution.Rating5Count,
            r.PublicationDate
        );
}