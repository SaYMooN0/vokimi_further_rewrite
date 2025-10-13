namespace UserProfilesService.Application.app_users.commands;

public sealed record SaveBasicProfileSetupCommand() : ICommand;

internal sealed class SaveBasicProfileSetupCommandHandler :
    ICommandHandler<SaveBasicProfileSetupCommand>
{
    public  Task<ErrOrNothing> Handle(SaveBasicProfileSetupCommand command, CancellationToken ct) {
        throw new NotImplementedException();
    }
}