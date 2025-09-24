using GeneralVokiTakingService.Domain.general_voki_aggregate;
using SharedKernel.auth;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Application.general_vokis.queries;

public sealed record VokiReceivedResultsQuery(VokiId VokiId) : IQuery<VokiReceivedResultsQueryResult>;

internal sealed class VokiReceivedResultsQueryHandler : IQueryHandler<VokiReceivedResultsQuery, VokiReceivedResultsQueryResult>
{
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserContext _userContext;
    private readonly IGeneralVokiTakenRecordsRepository _generalVokiTakenRecordsRepository;

    public VokiReceivedResultsQueryHandler(
        IGeneralVokisRepository generalVokisRepository,
        IUserContext userContext,
        IGeneralVokiTakenRecordsRepository generalVokiTakenRecordsRepository
    ) {
        _generalVokisRepository = generalVokisRepository;
        _userContext = userContext;
        _generalVokiTakenRecordsRepository = generalVokiTakenRecordsRepository;
    }

    public async Task<ErrOr<VokiReceivedResultsQueryResult>> Handle(VokiReceivedResultsQuery previewQuery, CancellationToken ct) {
        AppUserId userId = _userContext.AuthenticatedUserId;


        GeneralVoki? voki = await _generalVokisRepository
            .GetWithResultsByIdAsNoTracking(previewQuery.VokiId, ct);

        if (voki is null) {
            return ErrFactory.NotFound.GeneralVoki();
        }

        var records = await _generalVokiTakenRecordsRepository
            .ForVokiByUserAsNoTracking(previewQuery.VokiId, userId, ct);

        if (records.Length == 0) {
            return new VokiReceivedResultsQueryResult(
                [],
                voki.InteractionSettings.ResultsVisibility,
                voki.Name,
                voki.ResultsCount
            );
        }

        var receivedResultIds = records
            .Select(r => r.ReceivedResultId)
            .ToImmutableHashSet();

        IEnumerable<VokiResult> results = voki.ResultsReceivedByUser(receivedResultIds);

        var takingsByResultId = records
            .GroupBy(r => r.ReceivedResultId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(r => (r.StartTime, r.FinishTime))
                    .OrderByDescending(t => t.StartTime)
                    .ToArray()
            );
        ImmutableArray<VokiResultWithTakingDates> resultsToReturn = results
            .Select(r => new VokiResultWithTakingDates(r, takingsByResultId.GetValueOrDefault(r.Id, defaultValue: [])))
            .OrderByDescending(x => x.VokiTakings.Length)
            .ToImmutableArray();

        return new VokiReceivedResultsQueryResult(
            resultsToReturn,
            voki.InteractionSettings.ResultsVisibility,
            voki.Name,
            voki.ResultsCount
        );
    }
}

public sealed record VokiReceivedResultsQueryResult(
    ImmutableArray<VokiResultWithTakingDates> Results,
    GeneralVokiResultsVisibility ResultsVisibility,
    VokiName VokiName,
    uint TotalResultsCount
);

public record VokiResultWithTakingDates(VokiResult Result, (DateTime Start, DateTime Finish)[] VokiTakings);