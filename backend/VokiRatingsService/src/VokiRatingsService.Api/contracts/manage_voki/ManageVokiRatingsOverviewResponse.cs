using VokiRatingsService.Domain.common;

namespace VokiRatingsService.Api.contracts.manage_voki;

public record ManageVokiRatingsOverviewResponse(
    RatingsDistributionResponse Distribution
) : ICreatableResponse<VokiRatingsDistribution>
{
    public static ICreatableResponse<VokiRatingsDistribution> Create(VokiRatingsDistribution r) =>
        new ManageVokiRatingsOverviewResponse(RatingsDistributionResponse.Create(r));
}