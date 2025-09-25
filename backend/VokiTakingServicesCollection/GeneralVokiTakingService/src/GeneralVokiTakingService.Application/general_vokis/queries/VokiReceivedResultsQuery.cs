using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using SharedKernel.auth;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using VokiTakingServicesLib.Domain.common;

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


        GeneralVoki? voki = await _generalVokisRepository.GetWithResultsByIdAsNoTracking(previewQuery.VokiId, ct);

        if (voki is null) {
            return ErrFactory.NotFound.GeneralVoki();
        }

        GeneralVokiTakenRecord[] records = await _generalVokiTakenRecordsRepository
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


        return new VokiReceivedResultsQueryResult(
            GatherVokiTakenRecordsForResult(records, voki.ResultsReceivedByUser(receivedResultIds)),
            voki.InteractionSettings.ResultsVisibility,
            voki.Name,
            voki.ResultsCount
        );
    }

    private ImmutableArray<(VokiResult, GeneralVokiTakenRecord[])> GatherVokiTakenRecordsForResult(
        GeneralVokiTakenRecord[] records,
        IEnumerable<VokiResult> results
    ) {
        Dictionary<GeneralVokiResultId, GeneralVokiTakenRecord[]> takingsByResultId = records
            .GroupBy(r => r.ReceivedResultId)
            .ToDictionary(
                g => g.Key,
                g => g.OrderByDescending(r => r.StartTime).ToArray()
            );
        return results
            .Select(r => (r, takingsByResultId.GetValueOrDefault(r.Id, defaultValue: [])))
            .OrderByDescending(tuple => tuple.Item2.Length)
            .ToImmutableArray();
    }
}

public sealed record VokiReceivedResultsQueryResult(
    ImmutableArray<(VokiResult Result, GeneralVokiTakenRecord[] Records)> ResultsWithTakings,
    GeneralVokiResultsVisibility ResultsVisibility,
    VokiName VokiName,
    uint TotalResultsCount
);