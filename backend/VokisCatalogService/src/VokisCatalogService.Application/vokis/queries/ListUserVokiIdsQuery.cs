using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using VokisCatalogService.Application.common.repositories;

namespace VokisCatalogService.Application.vokis.queries;

public sealed record ListIdsOfVokiAuthoredByUser() :
    IQuery<VokiId[]>,
    IWithAuthCheckStep;

internal sealed class ListIdsOfVokiAuthoredByUserHandler : IQueryHandler<ListIdsOfVokiAuthoredByUser, VokiId[]>
{
    private readonly IBaseVokisRepository _baseVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public ListIdsOfVokiAuthoredByUserHandler(IBaseVokisRepository baseVokisRepository, IUserCtxProvider userCtxProvider) {
        _baseVokisRepository = baseVokisRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOr<VokiId[]>> Handle(ListIdsOfVokiAuthoredByUser query, CancellationToken ct)=>
       await _baseVokisRepository.ListVokiAuthoredByUserIdsOrderByCreationDate(query.UserCtx(_userCtxProvider), ct);
}