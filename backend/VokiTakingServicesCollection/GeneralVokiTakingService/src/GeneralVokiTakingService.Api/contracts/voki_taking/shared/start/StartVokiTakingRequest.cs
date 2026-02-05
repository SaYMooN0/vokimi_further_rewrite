
namespace GeneralVokiTakingService.Api.contracts.voki_taking.shared.start;

public record StartVokiTakingRequest(
    bool TerminateCurrentActive
) : IRequestWithValidationNeeded
{
    public ErrOrNothing Validate() => ErrOrNothing.Nothing;
}
