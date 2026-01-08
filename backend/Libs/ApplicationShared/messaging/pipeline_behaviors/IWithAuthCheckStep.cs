using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;
using SharedKernel.user_ctx;

namespace ApplicationShared.messaging.pipeline_behaviors;

public interface IWithAuthCheckStep
{
    public virtual Err UnauthenticatedErr => ErrFactory.NoAccess("Access denied. Authentication required");
}

public static class AuthCheckStepHandler
{
    public sealed class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse> where TCommand :
        ICommand<TResponse>,
        IWithAuthCheckStep
    {
        private readonly ICommandHandler<TCommand, TResponse> _innerHandler;
        private readonly IUserContext _userContext;

        public CommandHandler(ICommandHandler<TCommand, TResponse> innerHandler, IUserContext userContext) {
            _innerHandler = innerHandler;
            _userContext = userContext;
        }

        public async Task<ErrOr<TResponse>> Handle(TCommand command, CancellationToken ct) {
            ErrOr<AppUserId> userIdOrErr = _userContext.UserIdFromToken();

            if (userIdOrErr.IsErr()) {
                return command.UnauthenticatedErr;
            }

            return await _innerHandler.Handle(command, ct);
        }
    }

    public sealed class CommandBaseHandler<TCommand> : ICommandHandler<TCommand> where TCommand :
        ICommand,
        IWithAuthCheckStep
    {
        private readonly ICommandHandler<TCommand> _innerHandler;
        private readonly IUserContext _userContext;


        public CommandBaseHandler(ICommandHandler<TCommand> innerHandler, IUserContext userContext) {
            _innerHandler = innerHandler;
            _userContext = userContext;
        }

        public async Task<ErrOrNothing> Handle(TCommand command, CancellationToken ct) {
            ErrOr<AppUserId> userIdOrErr = _userContext.UserIdFromToken();

            if (userIdOrErr.IsErr()) {
                return command.UnauthenticatedErr;
            }

            return await _innerHandler.Handle(command, ct);
        }
    }

    public sealed class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse> where TQuery :
        IQuery<TResponse>,
        IWithAuthCheckStep
    {
        private readonly IQueryHandler<TQuery, TResponse> _innerHandler;
        private readonly IUserContext _userContext;


        public QueryHandler(IQueryHandler<TQuery, TResponse> innerHandler, IUserContext userContext)  {
            _innerHandler = innerHandler;
            _userContext = userContext;
        }

        public async Task<ErrOr<TResponse>> Handle(TQuery query, CancellationToken ct) {
            ErrOr<AppUserId> userIdOrErr = _userContext.UserIdFromToken();

            if (userIdOrErr.IsErr()) {
                return query.UnauthenticatedErr;
            }

            return await _innerHandler.Handle(query, ct);
        }
    }
}