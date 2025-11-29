namespace SharedKernel;

public interface IAuthenticatedUserContext
{
    public AppUserId UserId { get; }
}