using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Api.contracts;

public record class UserRatedVokiIdsResponse(
    Dictionary<string, DateTime> VokiIdWithRatingDate
) : ICreatableResponse<VokiIdWithRatingDateDto[]>
{
    public static ICreatableResponse<VokiIdWithRatingDateDto[]> Create(VokiIdWithRatingDateDto[] vokis) =>
        new UserRatedVokiIdsResponse(
            vokis.ToDictionary(v => v.VokiId.ToString(), v => v.Date)
        );
}