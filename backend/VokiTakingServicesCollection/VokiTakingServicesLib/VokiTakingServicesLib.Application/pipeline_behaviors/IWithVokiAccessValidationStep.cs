using ApplicationShared.messaging;
using SharedKernel.auth;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;
using VokiTakingServicesLib.Application.repositories;
using VokiTakingServicesLib.Domain.base_voki_aggregate;

namespace VokiTakingServicesLib.Application.pipeline_behaviors;

public interface IWithVokTakingAccessValidationStep
{
    public VokiId VokiId { get; }
    public virtual Err NoAccessErr => ErrFactory.NoAccess("You do not have access to the voki");

    public virtual Err VokiNotFoundErr => ErrFactory.NotFound.Voki(
        "Requested voki was not found",
        $"Voki with id {VokiId} not found"
    );
}

internal static class VokTakingAccessValidationStepHandler
{
    internal sealed class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>, IWithVokTakingAccessValidationStep
    {
        private readonly IBaseVokisRepository _baseVokisRepository;
        private readonly IUserContext _userContext;
        private readonly ICommandHandler<TCommand, TResponse> _innerHandler;


        public CommandHandler(
            IBaseVokisRepository baseVokisRepository,
            IUserContext userContext,
            ICommandHandler<TCommand, TResponse> innerHandler
        ) {
            _baseVokisRepository = baseVokisRepository;
            _userContext = userContext;
            _innerHandler = innerHandler;
        }

        public async Task<ErrOr<TResponse>> Handle(TCommand command, CancellationToken ct) {
            BaseVoki? voki = await _baseVokisRepository.GetByIdAsNoTracking(command.VokiId, ct);
            if (voki is null) {
                return command.VokiNotFoundErr;
            }

            if (voki.CheckUserAccessToTake(_userContext).IsErr(out var err)) {
                return err;
            }

            return await _innerHandler.Handle(command, ct);
        }
    }

    internal sealed class CommandBaseHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand, IWithVokTakingAccessValidationStep
    {
        private readonly IBaseVokisRepository _baseVokisRepository;
        private readonly IUserContext _userContext;
        private readonly ICommandHandler<TCommand> _innerHandler;

        public CommandBaseHandler(
            IBaseVokisRepository baseVokisRepository,
            IUserContext userContext,
            ICommandHandler<TCommand> innerHandler
        ) {
            _baseVokisRepository = baseVokisRepository;
            _userContext = userContext;
            _innerHandler = innerHandler;
        }


        public async Task<ErrOrNothing> Handle(TCommand command, CancellationToken ct) {
            BaseVoki? voki = await _baseVokisRepository.GetByIdAsNoTracking(command.VokiId, ct);
            if (voki is null) {
                return command.VokiNotFoundErr;
            }

            if (voki.CheckUserAccessToTake(_userContext).IsErr(out var err)) {
                return err;
            }

            return await _innerHandler.Handle(command, ct);
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>, IWithVokTakingAccessValidationStep
    {
        private readonly IBaseVokisRepository _baseVokisRepository;
        private readonly IUserContext _userContext;
        private readonly IQueryHandler<TQuery, TResponse> _innerHandler;

        public QueryHandler(
            IBaseVokisRepository baseVokisRepository,
            IUserContext userContext,
            IQueryHandler<TQuery, TResponse> innerHandler
        ) {
            _baseVokisRepository = baseVokisRepository;
            _userContext = userContext;
            _innerHandler = innerHandler;
        }

        public async Task<ErrOr<TResponse>> Handle(TQuery query, CancellationToken ct) {
            BaseVoki? voki = await _baseVokisRepository.GetByIdAsNoTracking(query.VokiId, ct);
            if (voki is null) {
                return query.VokiNotFoundErr;
            }

            if (voki.CheckUserAccessToTake(_userContext).IsErr(out var err)) {
                return err;
            }

            return await _innerHandler.Handle(query, ct);
        }
    }
}