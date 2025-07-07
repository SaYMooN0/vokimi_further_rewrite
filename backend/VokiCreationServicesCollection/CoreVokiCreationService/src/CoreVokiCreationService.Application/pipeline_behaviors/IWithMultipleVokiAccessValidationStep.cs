using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.common.interfaces.repositories;
using SharedKernel.auth;

namespace CoreVokiCreationService.Application.pipeline_behaviors;

public interface IWithMultipleVokiAccessValidationStep
{
    VokiId[] VokiIds { get; }

    Err NoAccessErr => ErrFactory.NoAccess("You do not have access to one or more of the requested vokis");
}

internal static class WithMultipleVokiAccessValidationStepHandler
{
    internal sealed class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>, IWithMultipleVokiAccessValidationStep
    {
        private readonly IAppUsersRepository _appUsersRepository;
        private readonly IUserContext _userContext;
        private readonly ICommandHandler<TCommand, TResponse> _innerHandler;

        public CommandHandler(
            IAppUsersRepository appUsersRepository,
            IUserContext userContext,
            ICommandHandler<TCommand, TResponse> innerHandler
        ) {
            _appUsersRepository = appUsersRepository;
            _userContext = userContext;
            _innerHandler = innerHandler;
        }

        public async Task<ErrOr<TResponse>> Handle(TCommand command, CancellationToken ct) {
            AppUserId userId = _userContext.AuthenticatedUserId;
            AppUser user = (await _appUsersRepository.GetByIdAsNoTracking(userId))!;

            var allAccessibleVokiIds = user.InitializedVokiIds
                .Concat(user.CoAuthoredVokiIds)
                .ToHashSet();

            var hasInaccessible = command.VokiIds.Any(id => !allAccessibleVokiIds.Contains(id));
            if (hasInaccessible) {
                return command.NoAccessErr;
            }

            return await _innerHandler.Handle(command, ct);
        }
    }

    internal sealed class CommandBaseHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand, IWithMultipleVokiAccessValidationStep
    {
        private readonly IAppUsersRepository _appUsersRepository;
        private readonly IUserContext _userContext;
        private readonly ICommandHandler<TCommand> _innerHandler;

        public CommandBaseHandler(
            IAppUsersRepository appUsersRepository,
            IUserContext userContext,
            ICommandHandler<TCommand> innerHandler
        ) {
            _appUsersRepository = appUsersRepository;
            _userContext = userContext;
            _innerHandler = innerHandler;
        }

        public async Task<ErrOrNothing> Handle(TCommand command, CancellationToken ct) {
            AppUserId userId = _userContext.AuthenticatedUserId;
            AppUser user = (await _appUsersRepository.GetByIdAsNoTracking(userId))!;

            var allAccessibleVokiIds = user.InitializedVokiIds
                .Concat(user.CoAuthoredVokiIds)
                .ToHashSet();

            var hasInaccessible = command.VokiIds.Any(id => !allAccessibleVokiIds.Contains(id));
            if (hasInaccessible) {
                return command.NoAccessErr;
            }

            return await _innerHandler.Handle(command, ct);
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>, IWithMultipleVokiAccessValidationStep
    {
        private readonly IAppUsersRepository _appUsersRepository;
        private readonly IUserContext _userContext;
        private readonly IQueryHandler<TQuery, TResponse> _innerHandler;

        public QueryHandler(
            IAppUsersRepository appUsersRepository,
            IUserContext userContext,
            IQueryHandler<TQuery, TResponse> innerHandler
        ) {
            _appUsersRepository = appUsersRepository;
            _userContext = userContext;
            _innerHandler = innerHandler;
        }

        public async Task<ErrOr<TResponse>> Handle(TQuery query, CancellationToken ct) {
            AppUserId userId = _userContext.AuthenticatedUserId;
            AppUser user = (await _appUsersRepository.GetByIdAsNoTracking(userId))!;

            var allAccessibleVokiIds = user.InitializedVokiIds
                .Concat(user.CoAuthoredVokiIds)
                .ToHashSet();

            var hasInaccessible = query.VokiIds.Any(id => !allAccessibleVokiIds.Contains(id));
            if (hasInaccessible) {
                return query.NoAccessErr;
            }

            return await _innerHandler.Handle(query, ct);
        }
    }
}
