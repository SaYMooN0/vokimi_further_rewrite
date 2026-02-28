using Microsoft.Extensions.Logging;
using SharedKernel;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_ratings_snapshot_aggregate;

namespace VokiRatingsService.Application.background_services.commands;

public record UpdateRatingsSnapshotsFromMarkersCommand() : ICommand<int>
{
    bool ICommand<int>.RequireTransaction => false;
}

internal class UpdateRatingsSnapshotsFromMarkersCommandHandler : ICommandHandler<UpdateRatingsSnapshotsFromMarkersCommand, int>
{
    private readonly IUpdateRatingsSnapshotMarkerRepository _updateRatingsSnapshotMarkerRepository;
    private readonly IRatingsRepository _ratingsRepository;
    private readonly IVokiRatingsSnapshotRepository _vokiRatingsSnapshotRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<UpdateRatingsSnapshotsFromMarkersCommandHandler> _logger;

    public UpdateRatingsSnapshotsFromMarkersCommandHandler(
        IUpdateRatingsSnapshotMarkerRepository updateRatingsSnapshotMarkerRepository,
        IRatingsRepository ratingsRepository,
        IVokiRatingsSnapshotRepository vokiRatingsSnapshotRepository,
        IDateTimeProvider dateTimeProvider,
        ILogger<UpdateRatingsSnapshotsFromMarkersCommandHandler> logger
    ) {
        _updateRatingsSnapshotMarkerRepository = updateRatingsSnapshotMarkerRepository;
        _ratingsRepository = ratingsRepository;
        _vokiRatingsSnapshotRepository = vokiRatingsSnapshotRepository;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
    }

    public async Task<ErrOr<int>> Handle(UpdateRatingsSnapshotsFromMarkersCommand request, CancellationToken cancellationToken) {
        var markers = await _updateRatingsSnapshotMarkerRepository.GetBatch(100, cancellationToken);

        if (markers.Length == 0) {
            return 0;
        }

        var vokiIds = markers.Select(m => m.VokiId).Distinct().ToArray();
        var now = _dateTimeProvider.UtcNow;

        foreach (var vokiId in vokiIds) {
            try {
                var distribution = await _ratingsRepository.GetRatingsDistributionForVoki(vokiId, cancellationToken);
                var lastSnapshot =
                    await _vokiRatingsSnapshotRepository.GetLastSnapshotForVokiForUpdate(vokiId, cancellationToken);

                if (lastSnapshot is null) {
                    var newSnapshot = VokiRatingsSnapshot.CreateNew(vokiId, now, distribution);
                    await _vokiRatingsSnapshotRepository.Add(newSnapshot, cancellationToken);
                }
                else {
                    if (lastSnapshot.IsInSameDayAs(now)) {
                        lastSnapshot.Update(now, distribution);
                        await _vokiRatingsSnapshotRepository.Update(lastSnapshot, cancellationToken);
                    }
                    else {
                        var newSnapshot = VokiRatingsSnapshot.CreateNew(vokiId, now, distribution);
                        await _vokiRatingsSnapshotRepository.Add(newSnapshot, cancellationToken);
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while updating snapshot for Voki {VokiId}", vokiId.Value);
            }
        }

        await _updateRatingsSnapshotMarkerRepository.DeleteBatch(markers, cancellationToken);

        return markers.Length;
    }
}