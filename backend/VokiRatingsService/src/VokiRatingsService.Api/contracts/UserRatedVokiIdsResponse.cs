using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Api.contracts;

public record class UserRatedVokiIdsResponse(
    Dictionary<string, UserRatedVokiIdsResponse.RatingBriefDataResponse> VokiIdToLastRatingData
) : ICreatableResponse<VokiIdWithLastRatingDto[]>
{
    public static ICreatableResponse<VokiIdWithLastRatingDto[]> Create(VokiIdWithLastRatingDto[] vokis) =>
        new UserRatedVokiIdsResponse(vokis.ToDictionary(
            v => v.VokiId.ToString(),
            v => new RatingBriefDataResponse(v.Value, v.DateTime))
        );

    public record RatingBriefDataResponse(ushort Value, DateTime DateTime);
}