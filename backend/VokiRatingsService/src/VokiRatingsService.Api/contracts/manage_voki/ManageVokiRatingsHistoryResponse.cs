using VokiRatingsService.Application.vokis.queries;
using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;
using VokiRatingsService.Domain.voki_ratings_snapshot;

namespace VokiRatingsService.Api.contracts.manage_voki;

public record ManageVokiRatingsHistoryResponse(
    ManageVokiRatingsHistoryResponse.ManageVokiRatingsDailySnapshotResponse[] Snapshots,
    DateTime VokiPublicationDate
) : ICreatableResponse<ManageVokiRatingsHistoryQueryResult>
{
    public static ICreatableResponse<ManageVokiRatingsHistoryQueryResult> Create(
        ManageVokiRatingsHistoryQueryResult res
    ) => new ManageVokiRatingsHistoryResponse(
        res.Snapshots.Select(ManageVokiRatingsDailySnapshotResponse.FromSnapshot).ToArray(),
        res.VokiPublicationDate
    );

    public record ManageVokiRatingsDailySnapshotResponse(
        DateTime Date,
        RatingsDistributionResponse Distribution
    )
    {
        public static ManageVokiRatingsDailySnapshotResponse FromSnapshot(VokiRatingsSnapshot sn) => new(
            sn.Date, RatingsDistributionResponse.Create(sn.Distribution)
        );
    }
}