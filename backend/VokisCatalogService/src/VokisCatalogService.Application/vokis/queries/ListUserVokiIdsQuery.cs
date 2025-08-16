using SharedKernel.auth;
using VokisCatalogService.Domain.common.interfaces.repositories;

namespace VokisCatalogService.Application.vokis.queries;

public sealed record ListUserVokiIdsQuery() : IQuery<VokiId[]>;

internal sealed class ListUserVokiIdsQueryHandler : IQueryHandler<ListUserVokiIdsQuery, VokiId[]>
{
    private readonly IBaseVokisRepository _baseVokisRepository;
    private readonly IUserContext _userContext;

    public ListUserVokiIdsQueryHandler(IBaseVokisRepository baseVokisRepository, IUserContext userContext) {
        _baseVokisRepository = baseVokisRepository;
        _userContext = userContext;
    }

    public async Task<ErrOr<VokiId[]>> Handle(ListUserVokiIdsQuery query, CancellationToken ct) {
        AppUserId userId = _userContext.AuthenticatedUserId;
        return await _baseVokisRepository.ListVokiAuthoredByUserIdsOrderByCreationDate(userId);
    }
}