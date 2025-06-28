namespace AuthService.Domain.common.interfaces;

public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string passwordToCheck, string passwordHash);
}
