using SharedKernel.errs;

namespace ApplicationShared.messaging;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<ErrOr<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}