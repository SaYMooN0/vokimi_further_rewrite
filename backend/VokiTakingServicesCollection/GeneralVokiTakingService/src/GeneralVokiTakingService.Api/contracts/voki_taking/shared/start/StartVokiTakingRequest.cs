
namespace GeneralVokiTakingService.Api.contracts.voki_taking.shared.start;

public record StartVokiTakingRequest(
    bool TerminateExistingActiveSession
) : IRequestWithValidationNeeded
{
    public ErrOrNothing Validate() => ErrOrNothing.Nothing;
}
