using VokiRatingsService.Domain.common;

namespace VokiRatingsService.Api.contracts.manage_voki;

public record RatingsDistributionResponse(
    uint Rating1Count,
    uint Rating2Count,
    uint Rating3Count,
    uint Rating4Count,
    uint Rating5Count
)
{
    public static RatingsDistributionResponse Create(VokiRatingsDistribution d) => new(
        Rating1Count: d.Rating1Count,
        Rating2Count: d.Rating2Count,
        Rating3Count: d.Rating3Count,
        Rating4Count: d.Rating4Count,
        Rating5Count: d.Rating5Count
    );
}