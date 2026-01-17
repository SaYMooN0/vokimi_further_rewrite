using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;
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
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IVokiRatingsSnapshotRepository _ratingsSnapshotRepository;
    private readonly IVokisRepository _vokisRepository;
    public ManageVokiRatingsHistoryQueryHandler(IUserCtxProvider userCtxProvider, IVokiRatingsSnapshotRepository ratingsSnapshotRepository, IVokisRepository vokisRepository) {
        _userCtxProvider = userCtxProvider;
        _ratingsSnapshotRepository = ratingsSnapshotRepository;
        _vokisRepository = vokisRepository;
    }


    public async Task<ErrOr<ManageVokiRatingsHistoryQueryResult>> Handle(
        ManageVokiRatingsHistoryQuery query, CancellationToken ct
    ) {
        Voki? voki = await _vokisRepository.GetVokiById(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Voki does not exist");
        }

        if (!voki.CanUserManage(query.UserCtx(_userCtxProvider))) {
            return ErrFactory.NoAccess("To get this data you need to be a Voki manager");
        }

        VokiRatingsSnapshot[] snapshots =await _ratingsSnapshotRepository.ListSortedSnapshotsForVoki(query.VokiId, ct);
        
        return new ManageVokiRatingsHistoryQueryResult(snapshots, voki.PublicationDate);
    }
}

public sealed record ManageVokiRatingsHistoryQueryResult(
    VokiRatingsSnapshot[] Snapshots,
    DateTime VokiPublicationDate
);