using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedKernel;
using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Application.background_services;

public class RatingsSnapshotUpdaterBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<RatingsSnapshotUpdaterBackgroundService> _logger;
    private readonly IUpdateRatingsSnapshotMarkerRepository _updateRatingsSnapshotMarkerRepository;
    private readonly IRatingsRepository _ratingsRepository;
    private readonly IVokiRatingsSnapshotRepository _vokiRatingsSnapshotRepository;
    private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

    public RatingsSnapshotUpdaterBackgroundService(
        IServiceScopeFactory serviceScopeFactory,
        ILogger<RatingsSnapshotUpdaterBackgroundService> logger, IUpdateRatingsSnapshotMarkerRepository updateRatingsSnapshotMarkerRepository, IRatingsRepository ratingsRepository, IVokiRatingsSnapshotRepository vokiRatingsSnapshotRepository)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _updateRatingsSnapshotMarkerRepository = updateRatingsSnapshotMarkerRepository;
        _ratingsRepository = ratingsRepository;
        _vokiRatingsSnapshotRepository = vokiRatingsSnapshotRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Ratings snapshot updater background service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dateTimeProvider = scope.ServiceProvider.GetRequiredService<IDateTimeProvider>();

                var markers = await markerRepo.GetBatch(100, stoppingToken);

                if (markers.Length > 0)
                {
                    var vokiIds = markers.Select(m => m.VokiId).Distinct().ToArray();
                    var now = dateTimeProvider.UtcNow;

                    foreach (var vokiId in vokiIds)
                    {
                        try
                        {
                            var distribution = await ratingsRepo.GetRatingsDistributionForVoki(vokiId, stoppingToken);
                            var lastSnapshot = await snapshotRepo.GetLastSnapshotForVokiForUpdate(vokiId, stoppingToken);

                            if (lastSnapshot is null)
                            {
                                var newSnapshot = Domain.voki_ratings_snapshot_aggregate.VokiRatingsSnapshot.CreateNew(vokiId, now, distribution);
                                await snapshotRepo.Add(newSnapshot, stoppingToken);
                            }
                            else
                            {
                                if (lastSnapshot.IsInSameDayAs(now))
                                {
                                    lastSnapshot.Update(now, distribution);
                                    await snapshotRepo.Update(lastSnapshot, stoppingToken);
                                }
                                else
                                {
                                    var newSnapshot = Domain.voki_ratings_snapshot_aggregate.VokiRatingsSnapshot.CreateNew(vokiId, now, distribution);
                                    await snapshotRepo.Add(newSnapshot, stoppingToken);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error occurred while updating snapshot for Voki {VokiId}", vokiId.Value);
                        }
                    }

                    await markerRepo.DeleteBatch(markers, stoppingToken);

                    _logger.LogInformation("Processed {Count} snapshot markers at {Time}.", markers.Length, DateTime.UtcNow);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in RatingsSnapshotUpdaterBackgroundService.");
            }

            try
            {
                await Task.Delay(_interval, stoppingToken);
            }
            catch (TaskCanceledException) { }
        }

        _logger.LogInformation("Ratings snapshot updater background service stopped.");
    }
}