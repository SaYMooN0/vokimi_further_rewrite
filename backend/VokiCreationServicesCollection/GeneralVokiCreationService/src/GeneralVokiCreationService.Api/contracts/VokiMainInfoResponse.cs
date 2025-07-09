using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts;

public record class VokiMainInfoResponse()
{
    public static VokiMainInfoResponse Create(DraftGeneralVoki voki) => new ();
}