using AuthService.Application.common.repositories;
using SharedKernel;

namespace AuthService.Application.unconfirmed_users.commands;

public sealed record DeleteExpiredUnconfirmedUsersCommand() : ICommand;

internal sealed class
    DeleteExpiredUnconfirmedUsersCommandHandler : ICommandHandler<DeleteExpiredUnconfirmedUsersCommand>
{
    private readonly IUnconfirmedUsersRepository _unconfirmedUsersRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DeleteExpiredUnconfirmedUsersCommandHandler(
        IUnconfirmedUsersRepository unconfirmedUsersRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _unconfirmedUsersRepository = unconfirmedUsersRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOrNothing> Handle(DeleteExpiredUnconfirmedUsersCommand command, CancellationToken ct) {
        await _unconfirmedUsersRepository.DeleteAllExpiredUsers(_dateTimeProvider.UtcNow, ct);
        return ErrOrNothing.Nothing;
    }
}