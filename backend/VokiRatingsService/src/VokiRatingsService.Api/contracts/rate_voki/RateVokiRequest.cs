using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Api.contracts;

public class RateVokiRequest : IRequestWithValidationNeeded
{
    public ushort RatingValue { get; init; }

    public ErrOrNothing Validate() => RatingValueWithDate.CheckValueForErr(RatingValue);
    
}