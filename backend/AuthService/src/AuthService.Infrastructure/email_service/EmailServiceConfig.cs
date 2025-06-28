namespace AuthService.Infrastructure.email_service;

public class EmailServiceConfig
{
    public string Host { get; init; }
    public int Port { get; init; }
    public string Username { get; init; }
    public string Password { get; init; }
}