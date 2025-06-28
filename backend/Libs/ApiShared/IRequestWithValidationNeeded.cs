using SharedKernel.errs;

namespace ApiShared;

public interface IRequestWithValidationNeeded
{
    public ErrOrNothing Validate();
}
