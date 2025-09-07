using SharedKernel.common.vokis;
using SharedKernel.domain;
using SharedKernel.domain.ids;
using VokiTakingServicesLib.Domain.common;

namespace VokiTakingServicesLib.Domain.voki_taken_record_aggregate;

public abstract class BaseVokiTakenRecord : AggregateRoot<VokiTakenRecordId>
{
    protected BaseVokiTakenRecord() { }
    public VokiId TakenVokiId { get; }
    public AppUserId? VokiTakerId { get; }
    public abstract VokiType VokiType { get; }
    public DateTime TestTakingStart { get; }
    public DateTime TestTakingEnd { get; }

    protected BaseVokiTakenRecord(
        VokiTakenRecordId id,
        VokiId takenVokiId,
        AppUserId? vokiTakerId,
        DateTime testTakingStart,
        DateTime testTakingEnd
    ) {
        Id = id;
        TakenVokiId = takenVokiId;
        VokiTakerId = vokiTakerId;
        TestTakingStart = testTakingStart;
        TestTakingEnd = testTakingEnd;
        VokiTakerId = vokiTakerId;
    }
}