using SharedKernel.auth;
using VokisCatalogService.Application.common.repositories;

namespace VokisCatalogService.Application.vokis.queries;

public sealed record ListIdsOfVokiAuthoredByUser() : IQuery<VokiId[]>;

internal sealed class ListIdsOfVokiAuthoredByUserHandler : IQueryHandler<ListIdsOfVokiAuthoredByUser, VokiId[]>
{
    private readonly IBaseVokisRepository _baseVokisRepository;
    private readonly IUserContext _userContext;

    public ListIdsOfVokiAuthoredByUserHandler(IBaseVokisRepository baseVokisRepository, IUserContext userContext) {
        _baseVokisRepository = baseVokisRepository;
        _userContext = userContext;
    }

    public async Task<ErrOr<VokiId[]>> Handle(ListIdsOfVokiAuthoredByUser query, CancellationToken ct) {
        AppUserId userId = _userContext.AuthenticatedUserId;
        return await _baseVokisRepository.ListVokiAuthoredByUserIdsOrderByCreationDate(userId, ct);
    }
}