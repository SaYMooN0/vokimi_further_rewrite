using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.common;
using VokiRatingsService.Domain.voki_aggregate;
using VokiRatingsService.Domain.voki_ratings_snapshot;

namespace VokiRatingsService.Application.vokis.queries;

public sealed record ManageVokiRatingsDistributionQuery(
    VokiId VokiId
) : IQuery<VokiRatingsDistribution>,
    IWithAuthCheckStep;

internal sealed class ManageVokiRatingsDistributionQueryHandler :
    IQueryHandler<ManageVokiRatingsDistributionQuery, VokiRatingsDistribution>
{
    private readonly IUserContext _userContext;
    private readonly IRatingsRepository _ratingsRepository;
    private readonly IVokisRepository _vokisRepository;

    public ManageVokiRatingsDistributionQueryHandler(
        IUserContext userContext,
        IRatingsRepository ratingsRepository,
        IVokisRepository vokisRepository
    ) {
        _userContext = userContext;
        _ratingsRepository = ratingsRepository;
        _vokisRepository = vokisRepository;
    }


    public async Task<ErrOr<VokiRatingsDistribution>> Handle(
        ManageVokiRatingsDistributionQuery query, CancellationToken ct
    ) {
        var voki = await _vokisRepository.GetVokiManagerDto(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Voki does not exist");
        }

        if (!Voki.CanUserManage(_userContext.AuthenticatedUser, voki.PrimaryAuthorId, voki.ManagersIds)) {
            return ErrFactory.NoAccess("To get this data you need to be a Voki manager");
        }

        return await _ratingsRepository.GetRatingsDistributionForVoki(query.VokiId, ct);
    }
}