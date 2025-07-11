using ApiShared;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;

namespace VokimiStorageService.contracts;

public class RequestWithVokiId : IRequestWithValidationNeeded
{
    public string VokiId { get; init; }

    public ErrOrNothing Validate() => Guid.TryParse(VokiId, out Guid _)
        ? ErrOrNothing.Nothing
        : ErrFactory.IncorrectFormat("Incorrect voki id");

    public VokiId ParsedVokiId() => new(new(VokiId));
}