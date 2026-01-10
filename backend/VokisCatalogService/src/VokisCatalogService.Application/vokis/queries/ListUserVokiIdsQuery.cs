using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;
using VokisCatalogService.Application.common.repositories;

namespace VokisCatalogService.Application.vokis.queries;

public sealed record ListIdsOfVokiAuthoredByUser() :
    IQuery<VokiId[]>,
    IWithAuthCheckStep;

internal sealed class ListIdsOfVokiAuthoredByUserHandler : IQueryHandler<ListIdsOfVokiAuthoredByUser, VokiId[]>
{
    private readonly IBaseVokisRepository _baseVokisRepository;
    private readonly IUserCtx _userCtx;

    public ListIdsOfVokiAuthoredByUserHandler(IBaseVokisRepository baseVokisRepository, IUserCtx userCtx) {
        _baseVokisRepository = baseVokisRepository;
        _userCtx = userCtx;
    }

    public async Task<ErrOr<VokiId[]>> Handle(ListIdsOfVokiAuthoredByUser query, CancellationToken ct) {
        return await _baseVokisRepository.ListVokiAuthoredByUserIdsOrderByCreationDate(_userCtx.AuthenticatedUser, ct);
    }
}