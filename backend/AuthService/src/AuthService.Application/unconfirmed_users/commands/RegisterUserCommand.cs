using ApplicationShared.messaging;
using AuthService.Domain.common;
using AuthService.Domain.common.interfaces;
using AuthService.Domain.common.interfaces.repositories;
using AuthService.Domain.unconfirmed_user_aggregate;
using SharedKernel.common;

namespace AuthService.Application.unconfirmed_users.commands;

public sealed record RegisterUserCommand(AppUserName Username, Email Email, string Password) : ICommand;

internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
{
    private readonly IUnconfirmedUsersRepository _unconfirmedUsersRepository;
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(
        IUnconfirmedUsersRepository unconfirmedUsersRepository,
        IAppUsersRepository appUsersRepository,
        IPasswordHasher passwordHasher
    ) {
        _unconfirmedUsersRepository = unconfirmedUsersRepository;
        _appUsersRepository = appUsersRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrOrNothing> Handle(RegisterUserCommand command, CancellationToken ct) {
        if ((await AnyConfirmedUserWithEmail(command.Email)).IsErr(out var err)) {
            return err;
        }

        UnconfirmedUser? unconfirmedUser = await _unconfirmedUsersRepository.GetByEmail(command.Email);
        if (unconfirmedUser is null) {
            return await CreateNewUnconfirmedUser(command);
        }

        return await OverrideExistingUnconfirmedUser(unconfirmedUser, command);
    }


    private async Task<ErrOrNothing> CreateNewUnconfirmedUser(RegisterUserCommand command) {
        var creationRes = UnconfirmedUser.CreateNew(
            command.Username, command.Email, command.Password, _passwordHasher
        );
        if (creationRes.IsErr(out var err)) {
            return err;
        }

        await _unconfirmedUsersRepository.Add(creationRes.AsSuccess());
        return ErrOrNothing.Nothing;
    }

    private async Task<ErrOrNothing> OverrideExistingUnconfirmedUser(
        UnconfirmedUser unconfirmedUser, RegisterUserCommand command
    ) {
        var res = unconfirmedUser.Override(command.Username, command.Password, _passwordHasher);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _unconfirmedUsersRepository.Update(unconfirmedUser);
        return ErrOrNothing.Nothing;
    }

    private async Task<ErrOrNothing> AnyConfirmedUserWithEmail(Email email) {
        bool isEmailTaken = await _appUsersRepository.AnyUserWithEmail(email);
        if (isEmailTaken) {
            return ErrFactory.Conflict(
                "This email is already taken",
                $"Profile with email '{email}' already exists"
            );
        }

        return ErrOrNothing.Nothing;
    }

}