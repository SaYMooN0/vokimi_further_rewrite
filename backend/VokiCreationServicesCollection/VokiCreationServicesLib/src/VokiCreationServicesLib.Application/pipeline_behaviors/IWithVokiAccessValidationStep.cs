using ApplicationShared;
using ApplicationShared.messaging;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;
using SharedKernel.user_ctx;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace VokiCreationServicesLib.Application.pipeline_behaviors;

public interface IWithVokiAccessValidationStep : IWithAuthCheckStep
{
    public VokiId VokiId { get; }
    public virtual Err NoAccessErr => ErrFactory.NoAccess("You do not have access to the requested voki");

    public virtual Err VokiNotFoundErr => ErrFactory.NotFound.Voki(
        "Requested voki was not found",
        $"Voki with id {VokiId} not found"
    );
}

public static class VokiAccessValidationStepHandler
{
    public sealed class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse> where TCommand :
        ICommand<TResponse>,
        IWithVokiAccessValidationStep
    {
        private readonly IDraftVokiRepository _draftVokiRepository;
        private readonly IUserCtxProvider _userCtxProvider;
        private readonly ICommandHandler<TCommand, TResponse> _innerHandler;

        public CommandHandler(
            IDraftVokiRepository draftVokiRepository, IUserCtxProvider userCtxProvider,
            ICommandHandler<TCommand, TResponse> innerHandler
        ) {
            _draftVokiRepository = draftVokiRepository;
            _userCtxProvider = userCtxProvider;
            _innerHandler = innerHandler;
        }

        public async Task<ErrOr<TResponse>> Handle(TCommand command, CancellationToken ct) {
            BaseDraftVoki? voki = await _draftVokiRepository.GetById(command.VokiId, ct);
            if (voki is null) {
                return command.VokiNotFoundErr;
            }

            if (voki.HasUserAccess(command.UserCtx(_userCtxProvider))) {
                return await _innerHandler.Handle(command, ct);
            }

            return command.NoAccessErr;
        }
    }

    public sealed class CommandBaseHandler<TCommand> : ICommandHandler<TCommand> where TCommand :
        ICommand,
        IWithVokiAccessValidationStep
    {
        private readonly IDraftVokiRepository _draftVokiRepository;
        private readonly IUserCtxProvider _userCtxProvider;
        private readonly ICommandHandler<TCommand> _innerHandler;

        public CommandBaseHandler(
            IDraftVokiRepository draftVokiRepository,
            IUserCtxProvider userCtxProvider,
            ICommandHandler<TCommand> innerHandler
        ) {
            _draftVokiRepository = draftVokiRepository;
            _userCtxProvider = userCtxProvider;
            _innerHandler = innerHandler;
        }

        public async Task<ErrOrNothing> Handle(TCommand command, CancellationToken ct) {
            BaseDraftVoki? voki = await _draftVokiRepository.GetById(command.VokiId, ct);
            if (voki is null) {
                return command.VokiNotFoundErr;
            }

            if (voki.HasUserAccess(command.UserCtx(_userCtxProvider))) {
                return await _innerHandler.Handle(command, ct);
            }

            return command.NoAccessErr;
        }
    }

    public sealed class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse> where TQuery :
        IQuery<TResponse>,
        IWithVokiAccessValidationStep
    {
        private readonly IDraftVokiRepository _draftVokiRepository;
        private readonly IUserCtxProvider _userCtxProvider;
        private readonly IQueryHandler<TQuery, TResponse> _innerHandler;

        public QueryHandler(
            IDraftVokiRepository draftVokiRepository,
            IUserCtxProvider userCtxProvider,
            IQueryHandler<TQuery, TResponse> innerHandler
        ) {
            _draftVokiRepository = draftVokiRepository;
            _userCtxProvider = userCtxProvider;
            _innerHandler = innerHandler;
        }

        public async Task<ErrOr<TResponse>> Handle(TQuery query, CancellationToken ct) {
            BaseDraftVoki? voki = await _draftVokiRepository.GetById(query.VokiId, ct);

            if (voki is null) {
                return query.VokiNotFoundErr;
            }

            if (voki.HasUserAccess(query.UserCtx(_userCtxProvider))) {
                return await _innerHandler.Handle(query, ct);
            }

            return query.NoAccessErr;
        }
    }
}