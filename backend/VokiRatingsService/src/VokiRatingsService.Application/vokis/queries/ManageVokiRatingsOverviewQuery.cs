using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
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
    private readonly IUserContext _userContext;
    private readonly IRatingsRepository _ratingsRepository;
    private readonly IVokisRepository _vokisRepository;

    public ManageVokiRatingsOverviewQueryHandler(
        IUserContext userContext,
        IRatingsRepository ratingsRepository,
        IVokisRepository vokisRepository
    ) {
        _userContext = userContext;
        _ratingsRepository = ratingsRepository;
        _vokisRepository = vokisRepository;
    }


    public async Task<ErrOr<VokiRatingsDistribution>> Handle(
        ManageVokiRatingsOverviewQuery query, CancellationToken ct
    ) {
        Voki? voki = await _vokisRepository.GetVokiAsNoTrackingById(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Voki does not exist");
        }

        if (!voki.CanUserManage(_userContext.AuthenticatedUser)) {
            return ErrFactory.NoAccess("To get this data you need to be a Voki manager");
        }

        return await _ratingsRepository.GetRatingsDistributionForVoki(query.VokiId, ct);
    }
}