using ApplicationShared.messaging;
using AuthService.Application.unconfirmed_users.commands;
using Microsoft.Extensions.Hosting;

namespace AuthService.Infrastructure.background_services;

public class UnconfirmedUsersCleanupBackgroundService : BackgroundService
{
    private readonly ICommandHandler<DeleteExpiredUnconfirmedUsersCommand> _deleteExpiredCommandHandler;

    public UnconfirmedUsersCleanupBackgroundService(
        ICommandHandler<DeleteExpiredUnconfirmedUsersCommand> deleteExpiredCommandHandler
    ) {
        _deleteExpiredCommandHandler = deleteExpiredCommandHandler;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => throw new NotImplementedException();
}