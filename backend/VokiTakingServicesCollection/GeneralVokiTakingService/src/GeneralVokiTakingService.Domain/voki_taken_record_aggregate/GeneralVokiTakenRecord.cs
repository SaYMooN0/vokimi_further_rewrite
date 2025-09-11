using GeneralVokiTakingService.Domain.voki_taken_record_aggregate.events;
using VokiTakingServicesLib.Domain.common;
using VokiTakingServicesLib.Domain.voki_taken_record_aggregate;

namespace GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

public sealed class GeneralVokiTakenRecord : BaseVokiTakenRecord
{
    private GeneralVokiTakenRecord() { }
    public override VokiType VokiType => VokiType.General;
    public GeneralVokiResultId ReceivedResultId { get; }
    public ImmutableArray<VokiTakenQuestionDetails> QuestionDetails { get; }
    public bool WasWithSequentialAnswering { get; }

    private GeneralVokiTakenRecord(
        VokiTakenRecordId id,
        VokiId takenVokiId,
        AppUserId? vokiTakerId,
        DateTime startTime,
        DateTime finishTime,
        GeneralVokiResultId receivedResultId,
        ImmutableArray<VokiTakenQuestionDetails> questionDetails,
        bool wasWithSequentialAnswering
    ) : base(id, takenVokiId, vokiTakerId, startTime, finishTime) {
        ReceivedResultId = receivedResultId;
        QuestionDetails = questionDetails;
        WasWithSequentialAnswering = wasWithSequentialAnswering;
    }

    public static GeneralVokiTakenRecord CreateNew(
        VokiId takenVokiId, AppUserId? vokiTakerId,
        DateTime testTakingStart, DateTime finishTime,
        bool wasVokiWithForcedSequentialOrder,
        GeneralVokiResultId receivedResultId,
        ImmutableArray<VokiTakenQuestionDetails> questionDetails
    ) {
        GeneralVokiTakenRecord newRecord = new(
            VokiTakenRecordId.CreateNew(),
            takenVokiId, vokiTakerId,
            testTakingStart, finishTime,
            receivedResultId, questionDetails,
            wasVokiWithForcedSequentialOrder
        );
        newRecord.AddDomainEvent(new VokiTakenRecordCreatedEvent(
            newRecord.Id,
            newRecord.TakenVokiId,
            newRecord.VokiTakerId
        ));
        return newRecord;
    }
}