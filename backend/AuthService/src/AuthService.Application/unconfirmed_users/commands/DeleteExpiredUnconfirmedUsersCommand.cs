using AuthService.Application.common.repositories;
using SharedKernel;

namespace AuthService.Application.unconfirmed_users.commands;

public sealed record DeleteExpiredUnconfirmedUsersCommand() : ICommand<int>
{
    bool ICommand<int>.RequireTransaction => false;
}

internal sealed class DeleteExpiredUnconfirmedUsersCommandHandler :
    ICommandHandler<DeleteExpiredUnconfirmedUsersCommand, int>
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

    public async Task<ErrOr<int>> Handle(DeleteExpiredUnconfirmedUsersCommand command, CancellationToken ct) {
        return await _unconfirmedUsersRepository.DeleteAllExpiredUsers(_dateTimeProvider.UtcNow, ct);
    }
}