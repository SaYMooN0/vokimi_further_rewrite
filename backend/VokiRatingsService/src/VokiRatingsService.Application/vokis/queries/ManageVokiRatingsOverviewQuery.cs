using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_aggregate;

namespace VokiRatingsService.Application.vokis.queries;

public sealed record ManageVokiRatingsOverviewQuery(
    VokiId VokiId
) : IQuery<VokiRatingsDistribution>,
    IWithAuthCheckStep;

internal sealed class ManageVokiRatingsOverviewQueryHandler :
    IQueryHandler<ManageVokiRatingsOverviewQuery, VokiRatingsDistribution>
{
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IRatingsRepository _ratingsRepository;
    private readonly IVokisRepository _vokisRepository;

    public ManageVokiRatingsOverviewQueryHandler(
        IUserCtxProvider userCtxProvider,
        IRatingsRepository ratingsRepository,
        IVokisRepository vokisRepository
    ) {
        _userCtxProvider = userCtxProvider;
        _ratingsRepository = ratingsRepository;
        _vokisRepository = vokisRepository;
    }


    public async Task<ErrOr<VokiRatingsDistribution>> Handle(
        ManageVokiRatingsOverviewQuery query, CancellationToken ct
    ) {
        var voki = await _vokisRepository.GetVokiManagerDto(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Voki does not exist");
        }

        if (!Voki.CanUserManage(_userCtxProvider.AuthenticatedUser, voki.PrimaryAuthorId, voki.ManagersIds)) {
            return ErrFactory.NoAccess("To get this data you need to be a Voki manager");
        }

        return await _ratingsRepository.GetRatingsDistributionForVoki(query.VokiId, ct);
    }
}