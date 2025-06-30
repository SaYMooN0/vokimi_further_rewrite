namespace ApiShared;

public interface IRequestWithValidationNeeded
{
    public ErrOrNothing Validate();
}
