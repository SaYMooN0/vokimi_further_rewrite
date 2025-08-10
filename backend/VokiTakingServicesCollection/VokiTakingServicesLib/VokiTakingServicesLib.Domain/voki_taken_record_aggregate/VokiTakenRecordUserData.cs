using SharedKernel.domain;

namespace VokiTakingServicesLib.Domain.voki_taken_record_aggregate;

public class VokiTakenRecordUserData : ValueObject
{
    public override IEnumerable<object> GetEqualityComponents() => [];
}