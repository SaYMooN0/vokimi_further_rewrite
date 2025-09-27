using ApiShared;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Api.contracts;

public record class VokiRatedResponse(
    ushort Value,
    DateTime DateTime
) : ICreatableResponse<RatingValueWithDate>
{
    public static ICreatableResponse<RatingValueWithDate> Create(RatingValueWithDate success) => new VokiRatedResponse(
        success.Value,
        success.DateTime
    );
}