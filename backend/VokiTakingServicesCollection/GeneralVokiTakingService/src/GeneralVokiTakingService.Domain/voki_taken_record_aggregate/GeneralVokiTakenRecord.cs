using GeneralVokiTakingService.Domain.common;
using VokiTakingServicesLib.Domain.common;
using VokiTakingServicesLib.Domain.voki_taken_record_aggregate;

namespace GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

public sealed class GeneralVokiTakenRecord : BaseVokiTakenRecord
{
    private GeneralVokiTakenRecord() { }
    public override VokiType VokiType => VokiType.General;
    public GeneralVokiResultId ReceivedResultId { get; }
    public ImmutableArray<VokiTakenQuestionDetails> QuestionDetails { get; }
    public bool WasVokiWithForcedSequentialOrder { get; }

    private GeneralVokiTakenRecord(
        VokiTakenRecordId id,
        VokiId takenVokiId,
        AppUserId? vokiTakerId,
        DateTime testTakingStart,
        DateTime testTakingEnd,
        GeneralVokiResultId receivedResultId,
        ImmutableArray<VokiTakenQuestionDetails> questionDetails,
        bool wasVokiWithForcedSequentialOrder
    ) : base(id, takenVokiId, vokiTakerId, testTakingStart, testTakingEnd) {
        ReceivedResultId = receivedResultId;
        QuestionDetails = questionDetails;
        WasVokiWithForcedSequentialOrder = wasVokiWithForcedSequentialOrder;
    }

    public static GeneralVokiTakenRecord CreateNew(
        VokiTakingFinishedSuccessfullyData data
    ) {
        GeneralVokiTakenRecord newRecord = new(
            VokiTakenRecordId.CreateNew(),
            data.TakenVokiId,
            data.VokiTakerId,
            data.TestTakingStart,
            data.TestTakingEnd,
            data.ReceivedResultId,
            data.QuestionDetails,
            data.WasVokiWithForcedSequentialOrder
        );
        return newRecord;
    }

}