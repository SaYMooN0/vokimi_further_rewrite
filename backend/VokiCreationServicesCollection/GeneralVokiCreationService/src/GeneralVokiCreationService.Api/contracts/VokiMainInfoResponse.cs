using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts;

public record class VokiMainInfoResponse(
    string Name,
    string Cover,
    string[] Tags,
    VokiDetailsResponse Details
)
{
    public static VokiMainInfoResponse Create(DraftGeneralVoki voki) => new(
        voki.Name.ToString(),
        voki.Cover.ToString(),
        voki.Tags.Value.Select(t => t.ToString()).ToArray(),
        VokiDetailsResponse.FromDetails(voki.Details)
    );
}