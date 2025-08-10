using VokiTakingServicesLib.Domain.voki_taken_record_aggregate;

namespace GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

public sealed class GeneralVokiTakenRecord : BaseVokiTakenRecord
{
    public override VokiType VokiType => VokiType.General;
}