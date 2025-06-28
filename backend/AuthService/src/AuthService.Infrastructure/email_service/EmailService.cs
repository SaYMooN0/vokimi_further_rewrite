using AuthService.Application.abstractions;
using MailKit.Net.Smtp;
using MimeKit;

namespace AuthService.Infrastructure.email_service;

internal class EmailService : IEmailService
{
    private readonly string _host;
    private readonly int _port;
    private readonly string _username;
    private readonly string _password;
    private readonly FrontendConfig _frontendConfig;

    public EmailService(EmailServiceConfig config, FrontendConfig frontendConfig) {
        _host = config.Host;
        _port = config.Port;
        _username = config.Username;
        _password = config.Password;
        _frontendConfig = frontendConfig;
    }

    public Task<ErrOrNothing> SendRegistrationConfirmationLink(
        Email email, AppUserName username, UnconfirmedUserId userId, string confirmationCode
    ) {
        string link =
            $"{_frontendConfig.BaseUrl}/{_frontendConfig.ConfirmRegistrationPath}/{userId}/{confirmationCode}";
        string subject = "Please confirm your email";
        string body =
            $"""
                 <p>
                     Hi, {username}! Thank you for registering. Please click the link below to confirm your email:
                 </p>
                 <p>
                     <a href='{link}'>Confirm Email</a>
                 </p>
             """;
        return  SendEmailWithHtmlBody(to: email, subject, body);
    }

    private async Task<ErrOrNothing> SendEmailWithHtmlBody(
        Email to, string subject, string body
    ) {
        try {
            MimeMessage message = new();
            message.From.Add(new MailboxAddress("DoneZo", _username));
            message.To.Add(new MailboxAddress("", to.ToString()));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = body };

            using var client = await ConfigureSmtpClient();
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return ErrOrNothing.Nothing;
        }
        catch {
            return new Err("Could not establish connection to send an email");
        }
    }

    private async Task<SmtpClient> ConfigureSmtpClient() {
        var client = new SmtpClient();
        await client.ConnectAsync(_host, _port, true);
        await client.AuthenticateAsync(_username, _password);
        return client;
    }
}