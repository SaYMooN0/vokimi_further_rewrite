namespace GeneralVokiTakingService.Api.contracts;

public class ContinueVokiTakingRequest : IRequestWithValidationNeeded
{
    public string SessionId { get; init; }
    public ErrOrNothing Validate() => ;
}