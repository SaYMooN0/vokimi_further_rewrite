using ApplicationShared.messaging;
using SharedKernel.auth;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokiCreationServicesLib.Domain.repositories;

namespace VokiCreationServicesLib.Application.pipeline_behaviors;

public interface IWithVokiAccessValidationStep
{
    public VokiId VokiId { get; }
    public virtual Err NoAccessErr => ErrFactory.NoAccess("You do not have access to the requested voki");

    public virtual Err VokiNotFoundErr => ErrFactory.NotFound.Voki(
        "Requested voki was not found",
        $"Voki with id {VokiId} not found"
    );
}

internal static class VokiAccessValidationStepHandler
{
    internal sealed class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse> where TCommand :
        ICommand<TResponse>,
        IWithVokiAccessValidationStep
    {
        private readonly IDraftVokiRepository _draftVokiRepository;
        private readonly IUserContext _userContext;
        private readonly ICommandHandler<TCommand, TResponse> _innerHandler;

        public CommandHandler(
            IDraftVokiRepository draftVokiRepository, IUserContext userContext,
            ICommandHandler<TCommand, TResponse> innerHandler
        ) {
            _draftVokiRepository = draftVokiRepository;
            _userContext = userContext;
            _innerHandler = innerHandler;
        }

        public async Task<ErrOr<TResponse>> Handle(TCommand command, CancellationToken ct) {
            AppUserId userId = _userContext.AuthenticatedUserId;
            BaseDraftVoki? voki = await _draftVokiRepository.GetByIdAsNoTracking(command.VokiId);
            if (voki is null) {
                return command.VokiNotFoundErr;
            }

            if (!voki.HasAccessToEdit(userId)) {
                return command.NoAccessErr;
            }

            return await _innerHandler.Handle(command, ct);
        }
    }

    internal sealed class CommandBaseHandler<TCommand> : ICommandHandler<TCommand> where TCommand :
        ICommand,
        IWithVokiAccessValidationStep
    {
        private readonly IDraftVokiRepository _draftVokiRepository;
        private readonly IUserContext _userContext;
        private readonly ICommandHandler<TCommand> _innerHandler;

        public CommandBaseHandler(
            IDraftVokiRepository draftVokiRepository,
            IUserContext userContext,
            ICommandHandler<TCommand> innerHandler
        ) {
            _draftVokiRepository = draftVokiRepository;
            _userContext = userContext;
            _innerHandler = innerHandler;
        }

        public async Task<ErrOrNothing> Handle(TCommand command, CancellationToken ct) {
            AppUserId userId = _userContext.AuthenticatedUserId;
            BaseDraftVoki? voki = await _draftVokiRepository.GetByIdAsNoTracking(command.VokiId);
            if (voki is null) {
                return command.VokiNotFoundErr;
            }

            if (!voki.HasAccessToEdit(userId)) {
                return command.NoAccessErr;
            }

            return await _innerHandler.Handle(command, ct);
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse> where TQuery :
        IQuery<TResponse>,
        IWithVokiAccessValidationStep
    {
        private readonly IDraftVokiRepository _draftVokiRepository;
        private readonly IUserContext _userContext;
        private readonly IQueryHandler<TQuery, TResponse> _innerHandler;

        public QueryHandler(
            IDraftVokiRepository draftVokiRepository,
            IUserContext userContext,
            IQueryHandler<TQuery, TResponse> innerHandler
        ) {
            _draftVokiRepository = draftVokiRepository;
            _userContext = userContext;
            _innerHandler = innerHandler;
        }

        public async Task<ErrOr<TResponse>> Handle(TQuery query, CancellationToken ct) {
            AppUserId userId = _userContext.AuthenticatedUserId;
            BaseDraftVoki? voki = await _draftVokiRepository.GetByIdAsNoTracking(query.VokiId);
            
            if (voki is null) {
                return query.VokiNotFoundErr;
            }

            if (!voki.HasAccessToEdit(userId)) {
                return query.NoAccessErr;
            }

            return await _innerHandler.Handle(query, ct);
        }
    }
}