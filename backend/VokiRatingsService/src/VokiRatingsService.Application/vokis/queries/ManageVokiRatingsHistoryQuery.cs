using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_aggregate;
using VokiRatingsService.Domain.voki_ratings_snapshot;

namespace VokiRatingsService.Application.vokis.queries;

public sealed record ManageVokiRatingsHistoryQuery(
    VokiId VokiId
) : IQuery<ManageVokiRatingsHistoryQueryResult>,
    IWithAuthCheckStep;

internal sealed class ManageVokiRatingsHistoryQueryHandler :
    IQueryHandler<ManageVokiRatingsHistoryQuery, ManageVokiRatingsHistoryQueryResult>
{
    private readonly IUserContext _userContext;
    private readonly IVokiRatingsSnapshotRepository _ratingsSnapshotRepository;
    private readonly IVokisRepository _vokisRepository;
    public ManageVokiRatingsHistoryQueryHandler(IUserContext userContext, IVokiRatingsSnapshotRepository ratingsSnapshotRepository, IVokisRepository vokisRepository) {
        _userContext = userContext;
        _ratingsSnapshotRepository = ratingsSnapshotRepository;
        _vokisRepository = vokisRepository;
    }


    public async Task<ErrOr<ManageVokiRatingsHistoryQueryResult>> Handle(
        ManageVokiRatingsHistoryQuery query, CancellationToken ct
    ) {
        Voki? voki = await _vokisRepository.GetVokiAsNoTrackingById(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Voki does not exist");
        }

        if (!voki.CanUserManage(_userContext.AuthenticatedUser)) {
            return ErrFactory.NoAccess("To get this data you need to be a Voki manager");
        }

        VokiRatingsSnapshot[] snapshots =
            await _ratingsSnapshotRepository.ListSortedSnapshotsForVokiAsNoTracking(query.VokiId, ct);
        
        return new ManageVokiRatingsHistoryQueryResult(snapshots, voki.PublicationDate);
    }
}

public sealed record ManageVokiRatingsHistoryQueryResult(
    VokiRatingsSnapshot[] Snapshots,
    DateTime VokiPublicationDate
);